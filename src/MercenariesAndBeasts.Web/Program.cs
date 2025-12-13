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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources"); 
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("cs"),
    new CultureInfo("de")
};
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
var cs = builder.Configuration.GetConnectionString("MacGameDatabase");
builder.Services.AddDbContext<GameDbContext>(o => o.UseNpgsql(cs));

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
builder.Services.AddAuthorization();
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

    db.Database.Migrate();
    await GameSeed.SeedBaseContentAsync(db, userManager, roleManager);
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
