using QuickMed.DB;
using QuickMed.Interface;


namespace QuickMed.Services
{
    public class AdviceService : BaseServices, IAdvice
    {
        public AdviceService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<dynamic> GetAdviceMasterData()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblAdviceMaster>("TblAdviceMaster");
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public async Task<dynamic> GetAdviceTemplateData()
        {
            try
            {
                //return await _context.GetTableRowsAsync<TblAdviceTemplate>("TblAdviceTemplate");
                var q = @$"
                    SELECT * 
                    FROM TblAdviceTemplate at
                    WHERE NOT EXISTS (
                        SELECT 1 
                        FROM TblTreatmentTemplate tt 
                        WHERE tt.AdviceId = at.Id
                    )
                    AND NOT EXISTS (
                        SELECT 1 
                        FROM TblPrescription pr 
                        WHERE pr.AdviceId = at.Id
                    )
                    ORDER BY at.CreatedAt desc
                    ;";

                var Result = await _context.ExecuteSqlQueryAsync<TblAdviceTemplate>(q);

                return Result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> DeleteAdviceTemplete(string sql)
        {
            try
            {
                await _context.ExecuteSqlQueryAsync<TblAdviceTemplate>(sql);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> DeleteAdviceDetails(string sql)
        {
            try
            {
                await _context.ExecuteSqlQueryAsync<TblAdviceTemplateDetails>(sql);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> GetTemplateById(string sql)
        {
            try
            {
                var data = await _context.ExecuteSqlQueryAsync<TblAdviceTemplate>(sql);
                return data.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> GetTemplateDetailsById(string sql)
        {
            try
            {
                var data = await _context.ExecuteSqlQueryAsync<TblAdviceTemplateDetails>(sql);
                return data.ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveAdviceTemplate(TblAdviceTemplate data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblAdviceTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveAdviceTemplateDetails(List<TblAdviceTemplateDetails> data)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblAdviceTemplateDetails>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> UpdateAdviceTemplate(TblAdviceTemplate data)
        {
            try
            {
                var saveTemplate = await _context.UpdateAsync<TblAdviceTemplate>(data);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> GetOnlyAdviceData()
        {
            try
            {
                var q = @$"
                    SELECT * 
                    FROM TblAdviceTemplate at
                    WHERE NOT EXISTS (
                        SELECT 1 
                        FROM TblTreatmentTemplate tt 
                        WHERE tt.AdviceId = at.Id
                    )
                    AND NOT EXISTS (
                        SELECT 1 
                        FROM TblPrescription pr 
                        WHERE pr.AdviceId = at.Id
                    );";

                var Result = await _context.ExecuteSqlQueryAsync<TblAdviceTemplate>(q);

                return Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
