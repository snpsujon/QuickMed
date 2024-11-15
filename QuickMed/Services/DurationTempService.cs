using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class DurationTempService : BaseServices, IDurationTemp
    {
        public DurationTempService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> GetAsync()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblDuration>("TblDuration");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveAsync(TblDuration data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblDuration>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> UpdateAsync(TblDuration data)
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
        public async Task<dynamic> DeleteAsync(Guid id)
        {
            try
            {
                var q = $"DELETE FROM TblDuration WHERE Id = '{id}'";

                var deleteResult = await _context.ExecuteSqlQueryFirstorDefultAsync<TblDuration>(q);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
