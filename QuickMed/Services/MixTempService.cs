using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

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

        public async Task<dynamic> GetMasterDataById(string Id)
        {
            try
            {
                var sql = $@"
                            Select * From TblMixTemplate Where Id = '{Id}'
                        ";
                var data = await _context.ExecuteSqlQueryAsync<TblMixTemplate>(sql);
                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> DeleteAsync(Guid id)
        {
            try
            {
                var master = $"DELETE FROM TblMixTemplate WHERE Id = '{id}'";
                var details = $"DELETE FROM TblMixTempDetails WHERE TblMixTemplateMasterId = '{id}'";

                await _context.ExecuteSqlQueryFirstorDefultAsync<TblMixTemplate>(master);
                await _context.ExecuteSqlQueryFirstorDefultAsync<TblMixTempDetails>(details);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<dynamic> GetDetailsDataById(string Id)
        {
            try
            {
                var sql = $@"
                           select
                        dm.Id as BrandId,
                        ddd.Name || ' - ' || dm.Name || ' ( ' || dm.Strength || ' ) ' || ' - ' || dg.Name as BrandName,
                        dd.Id as DoseId,dd.Name as DoseName
                        from
                        TblMixTempDetails ttd
                        LEFT JOIN DrugMedicine dm on ttd.BrandId = dm.Id
                        LEFT JOIN DrugDosage ddd ON dm.DosageId = ddd.Id
                        LEFT JOIN DrugGeneric dg ON dm.GenericId = dg.Id
                        LEFT JOIN DrugManufacturer c ON dm.ManufacturerId = c.Id
                        LEFT JOIN TblDose dd ON ttd.DoseId = dd.Id
                        Where ttd.TblMixTemplateMasterId = '{Id}'
                        ";
                var data = await _context.ExecuteSqlQueryAsync<FavouriteDrugTempVM>(sql);
                return data;
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
