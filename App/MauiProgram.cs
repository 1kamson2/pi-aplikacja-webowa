using System.Diagnostics;
using App.Components.Widgets;
using CommunityToolkit.Maui;
using Core.Algorithm;
using Core.Database;
using Core.Histogram;
using Core.History;
using Core.Image.Processing;
using Core.Services;
using Core.Utility;
using Microsoft.Extensions.Logging;

namespace App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            ApplicationPaths.ApplicationDatabasePath = Path.Combine(
                FileSystem.AppDataDirectory,
                "ApplicationRespository.db"
            );
            Debug.WriteLine(
                $"[RELEASE] Created database at: {ApplicationPaths.ApplicationDatabasePath}"
            );

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
            ApplicationPaths.ApplicationDatabasePath = Path.Combine(
                FileSystem.AppDataDirectory,
                "DebugApplicationRespository.db"
            );
            Debug.WriteLine(
                $"[DEBUG] Created database at: {ApplicationPaths.ApplicationDatabasePath}"
            );
#endif
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<AlgorithmSingleton>();
            builder.Services.AddSingleton<HistogramSingleton>();
            builder.Services.AddSingleton<HistorySingleton>();
            builder.Services.AddSingleton<PngProcessor>();
            builder.Services.AddScoped<NotificationsService>();

            return builder.Build();
        }
    }
}
