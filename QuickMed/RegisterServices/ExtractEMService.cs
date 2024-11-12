﻿using QuickMed.Interface;
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
                
        }

    }
}
