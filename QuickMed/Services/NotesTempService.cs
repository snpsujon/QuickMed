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

        public async Task<dynamic> SaveAsync(TblNotesTemplate data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblNotesTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> UpdateAsync(TblNotesTemplate data)
        {
            try
            {
                int rowsAffected = await _context.UpdateAsync(data);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the template.", ex);
            }
        }
    }
}
