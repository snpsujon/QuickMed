using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class LicenseServices : BaseServices,ILicense
    {
        public LicenseServices(ApplicationDbContext context) : base(context)
        {
        }

        public Task<dynamic> ValidateLicense(string LicenseNo)
        {
            try
            {
                var data = _context.ExecuteSqlQueryAsync<dynamic>("select * from ");
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
