﻿using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class CCServices : BaseServices, ICCTemp
    {
        public CCServices(ApplicationDbContext context) : base(context)
        {
        }



        public async Task<dynamic> GetCCTempData()
        {
            try
            {
                return await _context.GetTableRowsAsync<TblCCTemplate>("TblCCTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveCCTemplate(TblCCTemplate data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblCCTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> UpdateCCTemplate(TblCCTemplate data)
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
                var q = $"DELETE FROM TblCCTemplate WHERE Id = '{id}'";

                var deleteResult = await _context.ExecuteSqlQueryFirstorDefultAsync<TblCCTemplate>(q);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
