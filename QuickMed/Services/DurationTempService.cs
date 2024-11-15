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
    }
}
