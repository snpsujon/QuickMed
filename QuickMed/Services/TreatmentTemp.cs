using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class TreatmentTemp : BaseServices, ITeatmentTemp
    {
        public TreatmentTemp(ApplicationDbContext context) : base(context)
        {
        }

        public Task<dynamic> ValidateLicense(string LicenseNo)
        {
            throw new NotImplementedException();
        }
    }
}
