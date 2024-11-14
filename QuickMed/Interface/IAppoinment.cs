using QuickMed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.Interface
{
    public interface IAppoinment : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> SaveAsync(PatientVM data);
        Task<dynamic> UpdateAsync(PatientVM data);
        Task<dynamic> DeleteAsync(Guid id);


    }
}
