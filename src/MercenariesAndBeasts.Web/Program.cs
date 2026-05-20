using MercenariesAndBeasts.Domain.Interface;
using MercenariesAndBeasts.Infrastructure;
using MercenariesAndBeasts.Infrastructure.AI;
using MercenariesAndBeasts.Infrastructure.Auth;
using MercenariesAndBeasts.Infrastructure.Localization;
using MercenariesAndBeasts.Infrastructure.AI.Translations;
using MercenariesAndBeasts.Web.Components;
using ApexCharts;
using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.SessionStorage;
using SharedServices.Services;
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

builder.Services.AddScoped<SharedServices.ToastService>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddSingleton<ThemeService>(_ => new ThemeService(builder.Configuration));
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddApexCharts();
builder.Services.AddTransient<HttpInterceptorHandler>();
builder.Services.AddScoped<AdminUserService>();
builder.Services.AddScoped<PlayerLootService>();
builder.Services.AddScoped<FightHistoryService>();
builder.Services.AddScoped<IFightService, FightService>();
builder.Services.AddScoped<IStatAggregator, StatAggregator>();
builder.Services.AddSingleton<IErrorService, LogErrorService>();
builder.Services.AddScoped<IUserService<AppUser>, AspNetUserService<AppUser>>();
builder.Services.AddScoped<IAuditService, DbAuditService>();

builder.Services.AddHttpClient("Backend")
    .AddHttpMessageHandler<HttpInterceptorHandler>();

var openAiKey = builder.Configuration["OpenAI:ApiKey"];
if (string.IsNullOrWhiteSpace(openAiKey))
    throw new InvalidOperationException("OpenAI:ApiKey is not configured in appsettings or environment.");

// ChatGptAsker – jeden společný klient; musí být před AddMabTranslations
builder.Services.AddSingleton(sp =>
    new MercenariesAndBeasts.Infrastructure.AI.ChatGptAsker(
        apiKey: openAiKey,
        isSimple: true,
        maxParallelism: 5,
        maxRetries: 5,
        baseDelayMs: 750));

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
builder.Services.AddScoped<GameSeed>();
builder.Services.AddScoped<PlayerOnboardingService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();

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
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapMabCultureEndpoint();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContextMercenariesAndBeasts>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var seed = services.GetRequiredService<GameSeed>();

    await db.Database.MigrateAsync();
    await seed.SeedIdentityAsync(userManager, roleManager);
    await seed.SeedAsync(false);
}

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
