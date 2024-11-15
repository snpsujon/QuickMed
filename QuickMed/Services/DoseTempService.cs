using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class DoseTempService : BaseServices, IDoseTemp
    {
        public DoseTempService(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<dynamic> GetAsync()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblDose>("TblDose");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveAsync(TblDose data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblDose>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> UpdateAsync(TblDose data)
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
