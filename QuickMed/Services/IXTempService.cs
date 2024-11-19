﻿using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.Services
{
    public class IXTempService : BaseServices, IIXTemp
    {
        public IXTempService(ApplicationDbContext context) : base(context)
        {
        }

        public Task<dynamic> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> GetCCTempData()
        {
            throw new NotImplementedException();
        }
        public async Task<dynamic> SaveTemplateDetails(List<TblIXDetails> data)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblIXDetails>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveAsync(TblIXTemplate data)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblIXTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> UpdateAsync(TblIXTemplate data)
        {
            try
            {
                var saveTemplate = await _context.UpdateAsync<TblIXTemplate>(data);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> DeleteDetailsAsync(Guid id)
        {
            try
            {
                var details = $"DELETE FROM TblIXDetails WHERE TblIXTempMasterId = '{id}'";
                await _context.ExecuteSqlQueryFirstorDefultAsync<TblIXDetails>(details);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<dynamic> GetDataById(string Id)
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
    }
}
