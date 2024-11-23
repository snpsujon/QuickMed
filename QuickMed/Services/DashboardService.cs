using QuickMed.DB;
using QuickMed.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.Services
{
    public class DashboardService : BaseServices, IDashboard
    {
        public DashboardService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> GetMostUsedDXAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetMostUsedMedicineAsync()
        {
            throw new NotImplementedException();
        }
    }
}
