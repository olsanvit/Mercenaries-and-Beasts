using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedServices;
using SharedServices.Services;

namespace MercenariesAndBeasts.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Konfigurace z embedded appsettings.json
        var assembly = typeof(MauiProgram).Assembly;
        using var stream = assembly.GetManifestResourceStream("MercenariesAndBeasts.Mobile.appsettings.json");
        if (stream is not null)
        {
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(config);
        }

        // EF Core — přímé připojení k PostgreSQL
        var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Chybí DefaultConnection v appsettings.json");

        builder.Services.AddDbContextFactory<AppDbContextMercenariesAndBeasts>(opt =>
        {
            opt.UseNpgsql(connStr);
#if DEBUG
            opt.EnableDetailedErrors();
#endif
        });

        // Generic EF Core service + utility
        builder.Services.AddScoped<ToastService>();
        builder.Services.AddSingleton<ThemeService>();
        builder.Services.AddScoped<EfCoreService<AppDbContextMercenariesAndBeasts>>();

        return builder.Build();
    }
}
