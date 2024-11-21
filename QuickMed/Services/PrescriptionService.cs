using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.Services
{
    public class PrescriptionService : BaseServices, IPrescription
    {
        public PrescriptionService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> DeleteAsync(Guid id)
        {
            try
            {
                var master = $"DELETE FROM TblPrescription WHERE Id = '{id}'";

                await _context.ExecuteSqlQueryFirstorDefultAsync<TblNotesTemplate>(master);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<dynamic> GetAll()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblPrescription>("TblPrescription");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task<FavouriteDrugTempVM> GetFavDrugbyId(Guid id)
        {
            var gg = "";
            var sql = $@"select fd.Id,fd.Name,dm.Id as BrandId,
                        ddd.Name || ' - ' || dm.Name || ' ( ' || dm.Strength || ' ) ' || ' - ' || dg.Name as BrandName,
                        dd.Id as DoseId,dd.Name as DoseName,
                        d.Id as DurationId,d.Name as DurationName,i.Id as InstructionId,i.Name as InstructionName FROM TblFavouriteDrugTemplate fd
                        LEFT JOIN DrugMedicine dm on fd.BrandId = dm.Id

                        LEFT JOIN DrugDosage ddd ON dm.DosageId = ddd.Id
                        LEFT JOIN DrugGeneric dg ON dm.GenericId = dg.Id
                        LEFT JOIN DrugManufacturer c ON dm.ManufacturerId = c.Id

                        LEFT JOIN TblDose dd ON fd.DoseId = dd.Id
                        LEFT JOIN TblDuration d ON fd.DurationId = d.Id
                        LEFT JOIN TblInstruction i ON fd.InstructionId = i.Id WHERE fd.Id = '{id}'";
            return _context.ExecuteSqlQueryFirstorDefultAsync<FavouriteDrugTempVM>(sql);
        }

        public async Task<List<FavouriteDrugTempVM>> TblTreatmentTempDetails(Guid id)
        {

            var sql = $@"select
                        ttm.AdviceId as Id,dm.Id as BrandId,
                        ddd.Name || ' - ' || dm.Name || ' ( ' || dm.Strength || ' ) ' || ' - ' || dg.Name as BrandName,
                        dd.Id as DoseId,dd.Name as DoseName,
                        d.Id as DurationId,d.Name as DurationName,i.Id as InstructionId,i.Name as InstructionName
                        from
                        TblTreatmentTempDetails ttd
                        JOIN TblTreatmentTemplate ttm on ttd.TreatmentTempId = ttm.Id
                        LEFT JOIN DrugMedicine dm on ttd.BrandId = dm.Id
                        LEFT JOIN DrugDosage ddd ON dm.DosageId = ddd.Id
                        LEFT JOIN DrugGeneric dg ON dm.GenericId = dg.Id
                        LEFT JOIN DrugManufacturer c ON dm.ManufacturerId = c.Id
                        LEFT JOIN TblDose dd ON ttd.DoseId = dd.Id
                        LEFT JOIN TblDuration d ON ttd.DurationId = d.Id
                        LEFT JOIN TblInstruction i ON ttd.InstructionId = i.Id
                        WHERE ttd.TreatmentTempId = '{id}'";

            return (List<FavouriteDrugTempVM>)await _context.ExecuteSqlQueryAsync<FavouriteDrugTempVM>(sql);
        }
    }
}
