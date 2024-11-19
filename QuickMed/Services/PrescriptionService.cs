using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class PrescriptionService : BaseServices, IPrescription
    {
        public PrescriptionService(ApplicationDbContext context) : base(context)
        {
        }

        public Task<dynamic> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
