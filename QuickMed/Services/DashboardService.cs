using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.Services
{
    public class DashboardService : BaseServices, IDashboard
    {
        public DashboardService(ApplicationDbContext context) : base(context)
        {
        }

        //public async Task<dynamic> GetMostUsedDXAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<dynamic> GetMostUsedMedicineAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<dynamic> GetTotalDashboardData()
        {
            try
            {
                var sql = $@"SELECT 
                        (SELECT COUNT(Id) FROM TblPatient) AS TotalPatient,
                        (SELECT COUNT(Id) FROM TblPrescription) AS TotalPrescription,
                        (SELECT COUNT(Id) FROM DrugMedicine) AS TotalMedicine";
                var medicine = await _context.ExecuteSqlQueryAsync<TotalDashboardVM>(sql);
                return medicine.FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
