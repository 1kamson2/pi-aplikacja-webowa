using System.Diagnostics;
using CommunityToolkit.Maui;
using Core.Database;
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

            builder.Services.AddMauiBlazorWebView();
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
            DebugData.Apply();
#endif
            return builder.Build();
        }
    }
}
