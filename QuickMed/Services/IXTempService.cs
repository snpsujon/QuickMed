using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class IXTempService : BaseServices, IIXTemp
    {
        public IXTempService(ApplicationDbContext context) : base(context)
        {
        }

        public Task<dynamic> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> GetCCTempData()
        {
            throw new NotImplementedException();
        }
    }
}
