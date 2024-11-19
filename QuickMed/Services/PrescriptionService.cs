using QuickMed.DB;
using QuickMed.Interface;

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
    }
}
