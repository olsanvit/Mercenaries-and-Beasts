using FluentValidation;
using MercenariesAndBeasts.Domain.Interface;
using MudBlazor.Services;
using Radzen;
using MercenariesAndBeasts.Infrastructure;
using MercenariesAndBeasts.Infrastructure.AI;
using MercenariesAndBeasts.Infrastructure.Auth;
using MercenariesAndBeasts.Infrastructure.Localization;
using MercenariesAndBeasts.Infrastructure.AI.Translations;
using MercenariesAndBeasts.Web.Components;
using SharedServices;
using SharedServices.Services;
using MercenariesAndBeasts.Web.Achievements;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MercenariesAndBeasts.Infrastructure.Players;
using MercenariesAndBeasts.Infrastructure.Fights;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.PostgreSQL.ColumnWriters;
using SharedServices.Services.Common;
using Npgsql;
using System.Net;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Logs"));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .WriteTo.PostgreSQL(
        connectionString: builder.Configuration.GetConnectionString("QNAPGameDatabase") ?? "",
        tableName: "Logs",
        columnOptions: (IDictionary<string, ColumnWriterBase>?)null,
        needAutoCreateTable: true,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddMabLocalization<AppDbContextMercenariesAndBeasts>();

var csRaw = builder.Configuration.GetConnectionString("QNAPGameDatabase");
if (string.IsNullOrWhiteSpace(csRaw))
    throw new InvalidOperationException("Connection string 'QNAPGameDatabase' is missing.");

var cs = PreferIPv4Host(csRaw);

Log.Information("CS = {MaskedConnectionString}", Mask(cs));
Log.Information("DB Host resolved = {DbHostInfo}", DescribeHost(cs));

// NpgsqlDataSource — EnableDynamicJson + retry
var mabDsb = new NpgsqlDataSourceBuilder(cs);
mabDsb.EnableDynamicJson();
var mabDataSource = mabDsb.Build();

static string Mask(string? s)
{
    if (string.IsNullOrWhiteSpace(s)) return "(null)";
    return System.Text.RegularExpressions.Regex.Replace(s, "(?i)Password=([^;]+)", "Password=***");
}

static string DescribeHost(string cs)
{
    try
    {
        var b = new NpgsqlConnectionStringBuilder(cs);
        return $"{b.Host}:{b.Port} (Database={b.Database}, SSL={b.SslMode})";
    }
    catch
    {
        return "(unable to parse connection string)";
    }
}

/// <summary>
/// Rewrites the connection string so that the database host is resolved to an IPv4 address.
/// If the host is already a numeric IP it is returned unchanged; otherwise DNS is queried and
/// the first IPv4 result replaces the hostname, preventing connection failures on dual-stack systems.
/// </summary>
/// <param name="cs">The original PostgreSQL connection string.</param>
/// <returns>The connection string with the host replaced by an IPv4 address, or the original string if resolution fails.</returns>
static string PreferIPv4Host(string cs)
{
    var b = new NpgsqlConnectionStringBuilder(cs);

    // když už je to IP (v4/v6), neřeš
    if (IPAddress.TryParse(b.Host, out _))
        return b.ToString();

    // zkus DNS -> vyber IPv4
    try
    {
        var addrs = Dns.GetHostAddresses(b.Host!);
        var v4 = addrs.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
        if (v4 != null)
        {
            Log.Information("Host '{Host}' -> using IPv4 {IPv4}", b.Host, v4);
            b.Host = v4.ToString();
        }
        else
        {
            Log.Warning("Host '{Host}' -> no IPv4 found (only: {Addresses})", b.Host, string.Join(", ", addrs.Select(a => a.ToString())));
        }
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "DNS resolve failed for '{Host}'", b.Host);
    }

    return b.ToString();
}

// DbContextFactory + scoped DbContext — NpgsqlDataSource (EnableDynamicJson, retry 5×/5s, timeout 120s)
builder.Services.AddMabDbContext<AppDbContextMercenariesAndBeasts>(mabDataSource);

builder.Services.AddMabAuth<AppDbContextMercenariesAndBeasts>(builder.Configuration);

// Identity UI vyžaduje IEmailSender pro ExternalLogin flow — no-op implementace
builder.Services.AddSingleton<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender,
    NoOpEmailSender>();

builder.Services.AddMudServices();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<UiLibraryService>();
builder.Services.AddSharedUI(builder.Configuration);
builder.Services.AddScoped<AchievementService>(sp =>
    new AchievementService(
        sp.GetRequiredService<SharedServices.ToastService>(),
        sp.GetRequiredService<IWebHostEnvironment>())
    {
        Definitions = MercenariesAchievements.All
    });
builder.Services.AddTransient<HttpInterceptorHandler>();
builder.Services.AddScoped<AdminUserService>();
builder.Services.AddScoped<PlayerLootService>();
builder.Services.AddScoped<MercenariesAndBeasts.Web.Services.TestPlayerSeeder>();
builder.Services.AddScoped<MercenariesAndBeasts.Web.Services.TestDungeonsAndBeastsSeeder>();
builder.Services.AddScoped<MercenariesAndBeasts.Web.Services.TestLocationsAndMercenariesSeeder>();
builder.Services.AddScoped<FightHistoryService>();
builder.Services.AddScoped<MercenariesAndBeasts.Web.Services.LevelUpService>();
builder.Services.AddScoped<IFightService, FightService>();
builder.Services.AddScoped<IStatAggregator, StatAggregator>();
builder.Services.AddSingleton<IErrorService, LogErrorService>();
builder.Services.AddScoped<IUserService<AppUser>, AspNetUserService<AppUser>>();
builder.Services.AddScoped<IAuditService, DbAuditService>();

builder.Services.AddHttpClient("Backend")
    .AddHttpMessageHandler<HttpInterceptorHandler>();

var openAiKey = builder.Configuration["OpenAI:ApiKey"];
if (string.IsNullOrWhiteSpace(openAiKey))
    Log.Warning("OpenAI:ApiKey is not configured — AI translation features will be disabled.");
else
    builder.Services.AddSingleton(sp =>
        new MercenariesAndBeasts.Infrastructure.AI.ChatGptAsker(
            apiKey: openAiKey,
            isSimple: true,
            maxParallelism: 5,
            maxRetries: 5,
            baseDelayMs: 750));

if (!string.IsNullOrWhiteSpace(openAiKey))
{
    builder.Services.AddMabTranslations<AppDbContextMercenariesAndBeasts>(registry =>
    {
        registry.Add("Dungeon",           db => db.Dungeons.Select(x          => new ValueTuple<Guid, string, string?>(x.Guid, x.NameEn, x.DescriptionEn)));
        registry.Add("Location",          db => db.Locations.Select(x         => new ValueTuple<Guid, string, string?>(x.Guid, x.NameEn, x.DescriptionEn)));
        registry.Add("MonsterTemplate",   db => db.MonsterTemplates.Select(x   => new ValueTuple<Guid, string, string?>(x.Guid, x.NameEn, x.DescriptionEn)));
        registry.Add("MercenaryTemplate", db => db.MercenaryTemplates.Select(x => new ValueTuple<Guid, string, string?>(x.Guid, x.NameEn, x.DescriptionEn)));
        registry.Add("ItemTemplate",      db => db.ItemTemplates.Select(x      => new ValueTuple<Guid, string, string?>(x.Guid, x.NameEn, x.DescriptionEn)));
    });
    builder.Services.AddScoped<IUnitAiGenerator, AiUnitGeneratorService>();
    builder.Services.AddSingleton<IAiImageGenerator, AiImageGeneratorService>();
}
builder.Services.AddScoped<GameSeed>();
builder.Services.AddScoped<PlayerOnboardingService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<MercenariesAndBeasts.Web.Validators.StatScalingValidator>();

AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
    Log.Fatal(e.ExceptionObject as Exception, "UNHANDLED AppDomain exception");

TaskScheduler.UnobservedTaskException += (sender, e) =>
{
    Log.Fatal(e.Exception, "UNOBSERVED task exception");
    e.SetObserved();
};

var app = builder.Build();
app.MapHealthChecks("/health");
app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
var pathBase = builder.Configuration["PathBase"];
if (!string.IsNullOrWhiteSpace(pathBase))
    app.UsePathBase(pathBase);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseStaticFiles();     // kvůli CSS/skriptům z Identity UI
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorPages(); // kvůli Identity UI

// ── OAuth external login endpoints (Google / Facebook / Microsoft / GitHub / Apple) ──
app.MapPost("/Identity/Account/ExternalLogin", async (
    HttpContext http,
    SignInManager<AppUser> signInManager) =>
{
    var provider  = http.Request.Form["provider"].ToString();
    var returnUrl = http.Request.Form["returnUrl"].ToString() ?? "/";
    var callback  = $"/Identity/Account/ExternalLogin/Callback?returnUrl={Uri.EscapeDataString(returnUrl)}";
    var props     = signInManager.ConfigureExternalAuthenticationProperties(provider, callback);
    return Results.Challenge(props, new[] { provider });
}).DisableAntiforgery();

app.MapGet("/Identity/Account/ExternalLogin/Callback", async (
    HttpContext http,
    string? returnUrl,
    SignInManager<AppUser> signInManager,
    UserManager<AppUser> userManager,
    IWebHostEnvironment env,
    IConfiguration config) =>
{
    returnUrl ??= "/";
    var info = await signInManager.GetExternalLoginInfoAsync();
    if (info is null)
        return Results.Redirect("/login?error=external");

    var signIn = await signInManager.ExternalLoginSignInAsync(
        info.LoginProvider, info.ProviderKey, isPersistent: true);

    if (signIn.Succeeded)
    {
        var signedInUser = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        if (signedInUser is not null)
        {
            var denied = await AccessGate.CheckAsync(signedInUser, signInManager, env, config);
            if (denied is not null) return Results.Redirect(denied);
        }
        return Results.Redirect(returnUrl);
    }

    var email = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "";
    if (string.IsNullOrWhiteSpace(email))
        return Results.Redirect("/login?error=noemail");

    var user = new AppUser { UserName = email, Email = email };
    var created = await userManager.CreateAsync(user);
    if (created.Succeeded)
    {
        await userManager.AddLoginAsync(user, info);
        await signInManager.SignInAsync(user, isPersistent: true);
        var deniedNew = await AccessGate.CheckAsync(user, signInManager, env, config);
        if (deniedNew is not null) return Results.Redirect(deniedNew);
        return Results.Redirect(returnUrl);
    }

    var existing = await userManager.FindByEmailAsync(email);
    if (existing is not null)
    {
        await userManager.AddLoginAsync(existing, info);
        await signInManager.SignInAsync(existing, isPersistent: true);
        var deniedExisting = await AccessGate.CheckAsync(existing, signInManager, env, config);
        if (deniedExisting is not null) return Results.Redirect(deniedExisting);
        return Results.Redirect(returnUrl);
    }

    return Results.Redirect("/login?error=external");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapMabCultureEndpoint();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var db = services.GetRequiredService<AppDbContextMercenariesAndBeasts>();
        await db.Database.MigrateAsync();

        if (!string.IsNullOrWhiteSpace(openAiKey))
        {
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var seed = services.GetRequiredService<GameSeed>();
            await seed.SeedIdentityAsync(userManager, roleManager);
            await seed.SeedAsync(false);
        }
    }
}
catch (Exception ex) { Log.Warning(ex, "DB migration/seed skipped — DB not available"); }

// Seed role a admin účet
try
{
    await MercenariesAndBeasts.Infrastructure.Auth.AdminUserSeeder.SeedAsync(app.Services, app.Configuration);
}
catch (Exception ex) { Log.Warning(ex, "AdminUserSeeder skipped — DB not available"); }

app.Lifetime.ApplicationStopping.Register(() =>
    Log.Warning("Application stopping — flushing logs..."));

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

/// <summary>
/// A no-operation implementation of <see cref="Microsoft.AspNetCore.Identity.UI.Services.IEmailSender"/>
/// required by ASP.NET Core Identity UI for the external-login flow.
/// All send operations complete immediately without dispatching any email.
/// </summary>
file sealed class NoOpEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
        => Task.CompletedTask;
}

public partial class Program { }
