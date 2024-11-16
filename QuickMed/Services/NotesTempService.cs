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
        public async Task<dynamic> GetDetailsAsync()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblNotesTempDetails>("TblNotesTempDetails");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> GetDataById(string Id)
        {
            try
            {
                var sql = $"SELECT * FROM TblNotesTempDetails WHERE TblNotesTempMasterId = '{Id}'";
                var data = await _context.ExecuteSqlQueryAsync<TblNotesTempDetails>(sql);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<dynamic> DeleteAsync(Guid id)
        {
            try
            {
                var master = $"DELETE FROM TblNotesTemplate WHERE Id = '{id}'";
                var details = $"DELETE FROM TblNotesTempDetails WHERE TblNotesTempMasterId = '{id}'";

                 await _context.ExecuteSqlQueryFirstorDefultAsync<TblNotesTemplate>(master);
                 await _context.ExecuteSqlQueryFirstorDefultAsync<TblNotesTempDetails>(master);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<dynamic> SaveTemplateDetails(List<TblNotesTempDetails> data)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblNotesTempDetails>(data);
                return true;
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
