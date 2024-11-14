using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class DXTempServices : BaseServices, IDXTemp
    {
        public DXTempServices(ApplicationDbContext context) : base(context)
        {
        }



        public async Task<dynamic> GetCCTempData()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblDXTemplate>("TblDXTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveCCTemplate(TblDXTemplate data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblDXTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> UpdateCCTemplate(TblDXTemplate data)
        {
            try
            {
                int rowsAffected = await _context.UpdateAsync(data);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the template.", ex);
            }
        }
        public async Task<dynamic> DeleteAsync(Guid id)
        {
            try
            {
                var q = $"DELETE FROM TblDXTemplate WHERE Id = '{id}'";

                var deleteResult = await _context.ExecuteSqlQueryFirstorDefultAsync<TblDXTemplate>(q);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
