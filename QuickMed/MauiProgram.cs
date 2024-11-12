using Microsoft.Extensions.Logging;
using QuickMed.DB;
using QuickMed.RegisterServices;
using QuickMed.Services;

namespace QuickMed
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //            var builder = MauiApp.CreateBuilder();
            //            builder
            //                .UseMauiApp<App>()
            //                .ConfigureFonts(fonts =>
            //                {
            //                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            //                });


            //            builder.Services.AddMauiBlazorWebView();
            //            builder.Services.AddSingleton<ApplicationDbContext>();
            //            builder.Services.AddHostedService<DataSyncService>();

            //#if DEBUG
            //            builder.Services.AddBlazorWebViewDeveloperTools();
            //    		builder.Logging.AddDebug();
            //#endif

            //            return builder.Build();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });


            ExtractEMService.ExtractEMServices(builder.Services);
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<ApplicationDbContext>();
            //builder.Services.AddHostedService<DataSyncService>();
            builder.Services.AddSingleton<DataSyncService>();
            builder.Services.AddSingleton(new HttpClient());

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
