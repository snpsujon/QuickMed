using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class RefferenceService : BaseServices, IRefferenceTemp
    {
        public RefferenceService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> DeleteAsync(Guid id)
        {
            try
            {
                var sql = $"DELETE FROM TblRefferTemplate WHERE Id ={id}";
                await _context.ExecuteSqlQueryAsync<TblRefferTemplate>(sql);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> GetAsync()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblRefferTemplate>("TblRefferTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<dynamic> SaveAsync(TblRefferTemplate data)
        {
            try
            {
                await _context.CreateAsync<TblRefferTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<dynamic> UpdateAsync(TblRefferTemplate data)
        {
            try
            {
                await _context.UpdateAsync<TblRefferTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
