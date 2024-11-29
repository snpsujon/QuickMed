using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.Services
{
    public class TreatmentTemp : BaseServices, ITeatmentTemp
    {
        public TreatmentTemp(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> DeleteAllDetails(string TempId)
        {
            try
            {
                var sql = $"DELETE FROM TblTreatmentTempDetails WHERE TreatmentTempId = '{TempId}'";
                await _context.ExecuteSqlQueryAsync<TblTreatmentTempDetails>(sql);

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<dynamic> DeleteTreatmentTemp(string TempId)
        {
            var sql = $"SELECT * FROM TblTreatmentTemplate WHERE Id = '{TempId}'";
            var getTreatmentTemps = await _context.ExecuteSqlQueryAsync<TblTreatmentTemplate>(sql);

            if (getTreatmentTemps.Count() > 0)
            {
                //var getTreatmentTemp = getTreatmentTemps.FirstOrDefault();
                sql = $"DELETE FROM TblTreatmentTemplate WHERE Id = '{TempId}'";
                await _context.ExecuteSqlQueryAsync<TblTreatmentTemplate>(sql);
                sql = $"DELETE FROM TblTreatmentTempDetails WHERE TreatmentTempId = '{TempId}'";
                await _context.ExecuteSqlQueryAsync<TblTreatmentTempDetails>(sql);
                sql = $"DELETE FROM TblAdviceTemplate WHERE Id = '{getTreatmentTemps.First().AdviceId}'";
                await _context.ExecuteSqlQueryAsync<TblAdviceTemplate>(sql);
                sql = $"DELETE FROM TblAdviceTemplateDetails WHERE AdviceTemplateId = '{getTreatmentTemps.First().AdviceId}'";
                await _context.ExecuteSqlQueryAsync<TblAdviceTemplateDetails>(sql);

                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<dynamic> GetAdviceDataById(string AdviceID)
        {
            try
            {
                var sql = $"SELECT * FROM TblAdviceTemplateDetails WHERE AdviceTemplateId = '{AdviceID}'";
                var data = await _context.ExecuteSqlQueryAsync<TblAdviceTemplateDetails>(sql);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<dynamic> GetBrandsSameGenaric(string brandid)
        {
            try
            {
                var sql = $@"SELECT 
                    dm.Id as value,  
                    dd.Name || ' - ' || dm.Name || ' ( ' || dm.Strength || ' ) ' || ' - ' || dg.Name AS text
                    FROM DrugMedicine dm
                    LEFT JOIN DrugDosage dd ON dm.DosageId = dd.Id
                    LEFT JOIN DrugGeneric dg ON dm.GenericId = dg.Id
                    LEFT JOIN DrugManufacturer c ON dm.ManufacturerId = c.Id WHERE dm.GenericId = (SELECT GenericId FROM DrugMedicine WHERE Id = '{brandid}')";
                var getsameGeneric = await _context.ExecuteSqlQueryAsync<SelectVM>(sql);
                return getsameGeneric;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<dynamic> GetIXDataById(string Id)
        {
            try
            {
                var sql = $"SELECT * FROM TblIXDetails WHERE TblIXTempMasterId = '{Id}'";
                var data = await _context.ExecuteSqlQueryAsync<TblIXDetails>(sql);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<dynamic> GetNoteDataById(string Id)
        {
            try
            {
                var sql = $"SELECT * FROM TblNotesTempDetails WHERE TblNotesTempMasterId = '{Id}'";
                var data = await _context.ExecuteSqlQueryAsync<TblNotesTempDetails>(sql);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<dynamic> SaveTreatmentTemp(TblTreatmentTemplate data)
        {
            try
            {

                var saveTemplate = await _context.CreateAsync<TblTreatmentTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> UpdateTreatmentTemp(TblTreatmentTemplate data)
        {
            try
            {

                var updateTemplate = await _context.UpdateAsync<TblTreatmentTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<dynamic> SaveTreatmentTempDetails(List<TblTreatmentTempDetails> data)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblTreatmentTempDetails>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
