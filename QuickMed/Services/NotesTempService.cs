using QuickMed.DB;
using QuickMed.Interface;

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
                var q = @$"
                    SELECT * 
                    FROM TblNotesTemplate at
                    WHERE NOT EXISTS (
                        SELECT 1 
                        FROM TblPrescription pr 
                        WHERE pr.NoteId = at.Id
                    );";

                var Result = await _context.ExecuteSqlQueryAsync<TblNotesTemplate>(q);
                return Result;
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
                await _context.ExecuteSqlQueryFirstorDefultAsync<TblNotesTempDetails>(details);

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
        public async Task<dynamic> DeleteDetailsAsync(Guid id)
        {
            try
            {
                var details = $"DELETE FROM TblNotesTempDetails WHERE TblNotesTempMasterId = '{id}'";
                await _context.ExecuteSqlQueryFirstorDefultAsync<TblNotesTempDetails>(details);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
