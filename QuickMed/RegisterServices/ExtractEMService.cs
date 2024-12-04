using QuickMed.Interface;
using QuickMed.Services;
#if MACCATALYST
using QuickMed.MacOS;
#endif

namespace QuickMed.RegisterServices
{
    public class ExtractEMService
    {
        public static void ExtractEMServices(IServiceCollection services)
        {
            // Common services registration
            services.AddScoped<IBase, BaseServices>();
            services.AddScoped<IAdvice, AdviceService>();
            services.AddScoped<ILicense, LicenseServices>();
            services.AddScoped<ITeatmentTemp, TreatmentTemp>();
            services.AddScoped<ICCTemp, CCServices>();
            services.AddScoped<IDXTemp, DXTempServices>();
            services.AddScoped<IAppoinment, AppoinmentService>();
            services.AddScoped<IIXTemp, IXTempService>();
            services.AddScoped<INotesTemp, NotesTempService>();
            services.AddScoped<IDoseTemp, DoseTempService>();
            services.AddScoped<IMixTemp, MixTempService>();
            services.AddScoped<IDurationTemp, DurationTempService>();
            services.AddScoped<IFavouriteDrug, FavouriteDrugService>();
            services.AddScoped<IPrescription, PrescriptionService>();
            services.AddScoped<IRefferenceTemp, RefferenceService>();
            services.AddScoped<IDrug, DrugService>();
            services.AddScoped<IDashboard, DashboardService>();

            // macOS Catalyst-specific service registration
#if MACCATALYST
                        services.AddScoped<IPrinterMac, MacPrinter>();
#endif
        }
    }
}
