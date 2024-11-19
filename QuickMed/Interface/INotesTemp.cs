using QuickMed.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.Interface
{
    public interface INotesTemp : IBase
    {
        Task<dynamic> GetAsync();
        Task<dynamic> GetDetailsAsync();
        Task<dynamic> SaveAsync(TblNotesTemplate data);
        Task<dynamic> UpdateAsync(TblNotesTemplate data);
        Task<dynamic> SaveTemplateDetails(List<TblNotesTempDetails> data);
        Task<dynamic> DeleteAsync(Guid id);
        Task<dynamic> DeleteDetailsAsync(Guid id);
        Task<dynamic> GetDataById(string id);
    }
}
