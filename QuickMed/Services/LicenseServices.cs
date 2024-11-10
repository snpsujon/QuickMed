using QuickMed.Interface;

namespace QuickMed.Services
{
    public class LicenseServices : ILicense
    {
        private readonly ApplicationDbContext _dbContext;
        public LicenseServices()
        {

        }
        public Task<dynamic> ValidateLicense(string LicenseNo)
        {
            throw new NotImplementedException();
        }
    }
}
