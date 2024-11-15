using QuickMed.Interface;
using QuickMed.Services;

namespace QuickMed.RegisterServices
{
    public class ExtractEMService
    {
        public static void ExtractEMServices(IServiceCollection services)
        {
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

        }

    }
}
