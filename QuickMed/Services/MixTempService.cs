using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class MixTempService : BaseServices, IMixTemp
    {
        public MixTempService(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<dynamic> GetAsync()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblMixTemplate>("TblMixTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
