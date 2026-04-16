using MercenariesAndBeasts.Domain.Interface;
using MercenariesAndBeasts.Infrastructure;
using MercenariesAndBeasts.Infrastructure.AI;
using MercenariesAndBeasts.Infrastructure.Auth;
using MercenariesAndBeasts.Infrastructure.Localization;
using MercenariesAndBeasts.Infrastructure.AI.Translations;
using MercenariesAndBeasts.Web.Components;
using MercenariesAndBeasts.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MercenariesAndBeasts.Infrastructure.Players;
using MercenariesAndBeasts.Infrastructure.Fights;
using SharedServices.Services.Common;
using Npgsql;
using System.Net;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddMabLocalization<MercenariesAndBeastsDbContext>();

var csRaw = builder.Configuration.GetConnectionString("QNAPGameDatabase");
if (string.IsNullOrWhiteSpace(csRaw))
    throw new InvalidOperationException("Connection string 'QNAPGameDatabase' is missing.");

var cs = PreferIPv4Host(csRaw);

Console.WriteLine("CS = " + Mask(cs));
Console.WriteLine("DB Host resolved = " + DescribeHost(cs));

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
/// Pokud je Host hostname (ne IP), pokusí se ho přeložit a vynutit IPv4.
/// </summary>
static string PreferIPv4Host(string cs)
{
    var b = new NpgsqlConnectionStringBuilder(cs);

    // když už je to IP (v4/v6), neřeš
    if (IPAddress.TryParse(b.Host, out _))
        return b.ToString();

    // zkus DNS -> vyber IPv4
    try
    {
        var addrs = Dns.GetHostAddresses(b.Host);
        var v4 = addrs.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
        if (v4 != null)
        {
            Console.WriteLine($"Host '{b.Host}' -> using IPv4 {v4}");
            b.Host = v4.ToString();
        }
        else
        {
            Console.WriteLine($"Host '{b.Host}' -> no IPv4 found (only: {string.Join(", ", addrs.Select(a => a.ToString()))})");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"DNS resolve failed for '{b.Host}': {ex.Message}");
    }

    return b.ToString();
}

// DbContextFactory + scoped DbContext using IPv4-resolved connection string
builder.Services.AddMabDbContext<MercenariesAndBeastsDbContext>(cs);

builder.Services.AddMabAuth<MercenariesAndBeastsDbContext>(builder.Configuration);

builder.Services.AddSingleton<ErrorService>();
builder.Services.AddTransient<HttpInterceptorHandler>();
builder.Services.AddScoped<AdminUserService>();
builder.Services.AddScoped<PlayerLootService>();
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
    new ChatGptAsker(
        apiKey: openAiKey,
        isSimple: true,
        maxParallelism: 5,
        maxRetries: 5,
        baseDelayMs: 750));

builder.Services.AddMabTranslations<MercenariesAndBeastsDbContext>(registry =>
{
    registry.Add("Dungeon",           db => db.Dungeons.Select(x          => new ValueTuple<Guid, string, string?>(x.Id, x.NameEn, x.DescriptionEn)));
    registry.Add("Location",          db => db.Locations.Select(x         => new ValueTuple<Guid, string, string?>(x.Id, x.NameEn, x.DescriptionEn)));
    registry.Add("MonsterTemplate",   db => db.MonsterTemplates.Select(x   => new ValueTuple<Guid, string, string?>(x.Id, x.NameEn, x.DescriptionEn)));
    registry.Add("MercenaryTemplate", db => db.MercenaryTemplates.Select(x => new ValueTuple<Guid, string, string?>(x.Id, x.NameEn, x.DescriptionEn)));
    registry.Add("ItemTemplate",      db => db.ItemTemplates.Select(x      => new ValueTuple<Guid, string, string?>(x.Id, x.NameEn, x.DescriptionEn)));
});
builder.Services.AddScoped<IUnitAiGenerator, AiUnitGeneratorService>();
builder.Services.AddSingleton<IAiImageGenerator, AiImageGeneratorService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<GameSeed>();
builder.Services.AddScoped<PlayerOnboardingService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
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
    var db = services.GetRequiredService<MercenariesAndBeastsDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var seed = services.GetRequiredService<GameSeed>();

    await db.Database.MigrateAsync();
    await seed.SeedIdentityAsync(userManager, roleManager);
    await seed.SeedAsync(false);
}

app.Run();
