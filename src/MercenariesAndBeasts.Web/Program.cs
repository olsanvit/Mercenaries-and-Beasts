using MercenariesAndBeasts.Domain.Interface;
using MercenariesAndBeasts.Infrastructure;
using MercenariesAndBeasts.Infrastructure.AI;
using MercenariesAndBeasts.Web.Components;
using MercenariesAndBeasts.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAI;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Google;
using MercenariesAndBeasts.Infrastructure.Players;
using MercenariesAndBeasts.Domain.Localization;
using MercenariesAndBeasts.Infrastructure.Fights;
using Npgsql;
using System.Net;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
var supportedCultures = LocalizationConfig.SupportedLocales
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .Select(code =>
    {
        try
        {
            return CultureInfo.GetCultureInfo(code);
        }
        catch (CultureNotFoundException)
        {
            return null;
        }
    })
    .Where(c => c != null)
    .Cast<CultureInfo>()
    .ToArray();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // pořadí providerů: query string -> cookie -> browser
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});
//var cs = builder.Configuration.GetConnectionString("MacGameDatabase");


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
// Factory (singleton-safe)
builder.Services.AddDbContextFactory<GameDbContext>(options =>
{
    options.UseNpgsql(cs);
});

// Scoped DbContext pro Identity + běžné služby
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IDbContextFactory<GameDbContext>>().CreateDbContext());
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        // minimální nastavení, později doladíš
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<GameDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var clientId = builder.Configuration["Authentication:Google:ClientId"];
        var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            throw new InvalidOperationException("Missing Google OAuth config (Authentication:Google:ClientId/ClientSecret).");

        options.ClientId = clientId;
        options.ClientSecret = clientSecret;

        // důležité pro Identity external login flow
        options.SignInScheme = IdentityConstants.ExternalScheme;

        // default callback je /signin-google (nech tak)
        // options.CallbackPath = "/signin-google";
    });

builder.Services.AddSingleton<ErrorService>();
builder.Services.AddTransient<HttpInterceptorHandler>();
builder.Services.AddScoped<AdminUserService>();
builder.Services.AddScoped<PlayerLootService>();
builder.Services.AddScoped<IFightService, FightService>();
builder.Services.AddSingleton<IErrorService, LogErrorService>();

builder.Services.AddHttpClient("Backend")
    .AddHttpMessageHandler<HttpInterceptorHandler>();
var openAiKey = builder.Configuration["OpenAI:ApiKey"];
if (string.IsNullOrWhiteSpace(openAiKey))
{
    throw new InvalidOperationException("OpenAI:ApiKey is not configured in appsettings or environment.");
}

// ChatGptAsker – jeden společný klient
builder.Services.AddSingleton(sp =>
    new ChatGptAsker(
        apiKey: openAiKey,
        isSimple: true,
        maxParallelism: 5,
        maxRetries: 5,
        baseDelayMs: 750));

builder.Services.AddScoped<IUnitAiGenerator, AiUnitGeneratorService>();
builder.Services.AddSingleton<IAiImageGenerator, AiImageGeneratorService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<GameSeed>();
builder.Services.AddAuthorization();
builder.Services.AddScoped<PlayerOnboardingService>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
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

app.MapPost("/logout", async (SignInManager<AppUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
})
.DisableAntiforgery();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<GameDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var seed = services.GetRequiredService<GameSeed>();

    await db.Database.MigrateAsync();
    await seed.SeedIdentityAsync(userManager, roleManager); // tohle může zůstat static
    await seed.SeedAsync(false);                                     // instance metoda
}
app.MapGet("/set-culture", (string culture, string returnUrl, HttpContext httpContext) =>
{
    var requestCulture = new RequestCulture(culture);

    httpContext.Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(requestCulture),
        new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true
        });

    return Results.Redirect(returnUrl);
});
app.Run();
