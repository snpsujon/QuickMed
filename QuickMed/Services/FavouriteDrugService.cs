using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class FavouriteDrugService : BaseServices, IFavouriteDrug
    {
        public FavouriteDrugService(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<dynamic> GetAsync()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblFavouriteDrugTemplate>("TblFavouriteDrugTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
