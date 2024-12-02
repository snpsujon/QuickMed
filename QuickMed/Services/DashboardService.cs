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

        public async Task<dynamic> GetTotalDxDashboardData()
        {
            try
            {
                var sql = $@"SELECT dx.Name, COUNT(dx.Id) AS TotalPrescription
                        FROM TblPrescription P
                        JOIN TblPres_DX d ON d.Pres_ID = P.Id
                        JOIN TblDXTemplate dx ON dx.Id = d.DxTempId
                        GROUP BY dx.Id, dx.Name
                        ORDER BY TotalPrescription DESC
                        LIMIT 5";
                var medicine = await _context.ExecuteSqlQueryAsync<DxTempVM>(sql);
                return medicine.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> GetBrandDashboardData()
        {
            try
            {
                var sql = $@"SELECT 
                            dm.Name as BrandName, dg.Name as GenericName, c. Name as CompanyName
                            FROM TblPrescriptionDetails pd
                            LEFT JOIN DrugMedicine dm on dm.Id = pd.Brand
                            LEFT JOIN DrugGeneric dg ON dm.GenericId = dg.Id
                            LEFT JOIN DrugManufacturer c ON dm.ManufacturerId = c.Id 
                            GROUP by pd.Brand
                            ORDER by count(pd.Brand) DESC
                            LIMIT 5";
                var medicine = await _context.ExecuteSqlQueryAsync<BrandDashboardVM>(sql);
                return medicine.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
