using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.Services
{
    public class NotesTempService : BaseServices, INotesTemp
    {
        public NotesTempService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> GetAsync()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblNotesTemplate>("TblNotesTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
