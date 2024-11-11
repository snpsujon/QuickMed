using QuickMed.Interface;
using QuickMed.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.RegisterServices
{
    public class ExtractEMService
    {
        public static void ExtractEMServices(IServiceCollection services)
        {
            services.AddScoped<IBase, BaseServices>();
            services.AddScoped<IAdvice, AdviceService>();
            services.AddScoped<ILicense, LicenseServices>();
                
        }

    }
}
