using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.Interface
{
    public interface IDashboard
    {
        Task<dynamic> GetMostUsedDXAsync();
        Task<dynamic> GetMostUsedMedicineAsync();
    }
}
