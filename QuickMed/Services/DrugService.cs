using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.Services
{
    public class DrugService : BaseServices, IDrug
    {
        public DrugService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<DrugDbVM> GetDataByIdAsync(Guid? Id)
        {
            try
            {
                var sql = $@"
                         SELECT 
                          dm.Id,dm.Name,dg.Id as GenericId,dg.Name as GenericName,c.Id as ManufacturerId,c.Name as CompanyName, dm.PackageContainer, dm.PackageSize
                          FROM DrugMedicine dm
                          LEFT JOIN DrugGeneric dg ON dm.GenericId = dg.Id
                          LEFT JOIN DrugManufacturer c ON dm.ManufacturerId = c.Id 
                        Where dm.Id = '{Id}'";

                var result = await _context.ExecuteSqlQueryFirstorDefultAsync<DrugDbVM>(sql);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
