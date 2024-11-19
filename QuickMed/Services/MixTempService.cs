using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class MixTempService : BaseServices, IMixTemp
    {
        public MixTempService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> GetAllMedicine()
        {
            try
            {
                var sql = $@"
                   SELECT 
                    dm.Id,  
                    dd.Name || ' - ' || dm.Name || ' ( ' || dm.Strength || ' ) ' || ' - ' || dg.Name AS Name
                    FROM DrugMedicine dm
                    LEFT JOIN DrugDosage dd ON dm.DosageId = dd.Id
                    LEFT JOIN DrugGeneric dg ON dm.GenericId = dg.Id
                    LEFT JOIN DrugManufacturer c ON dm.ManufacturerId = c.Id;
                        ";
                var data = await _context.ExecuteSqlQueryAsync<DrugMedicine>(sql);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<dynamic> GetAsync()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblMixTemplate>("TblMixTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveMixTemp(TblMixTemplate tblTreatmentTemplate)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblMixTemplate>(tblTreatmentTemplate);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveMixTempDetails(List<TblMixTempDetails> data)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblMixTempDetails>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
