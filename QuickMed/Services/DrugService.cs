using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class DrugService : BaseServices, IDrug
    {
        public DrugService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
