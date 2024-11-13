using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class CCServices : BaseServices, ICCTemp
    {
        public CCServices(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> GetCCTempData()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblCCTemplate>("TblCCTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveCCTemplate(TblCCTemplate data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblCCTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
