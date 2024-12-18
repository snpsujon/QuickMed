﻿using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.Services
{
    public class FavouriteDrugService : BaseServices, IFavouriteDrug
    {
        public FavouriteDrugService(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<dynamic> GetAsync()
        {
            try
            {
                var query = @"
                        select fd.Id,fd.Name,dm.Id as BrandId,dm.Name as BrandName,dd.Id as DoseId,dd.Name as DoseName,
                        d.Id as DurationId,d.Name as DurationName,i.Id as InstructionId,i.Name as InstructionName FROM TblFavouriteDrugTemplate fd
                        LEFT JOIN DrugMedicine dm on fd.BrandId = dm.Id
                        LEFT JOIN TblDose dd ON fd.DoseId = dd.Id
                        LEFT JOIN TblDuration d ON fd.DurationId = d.Id
                        LEFT JOIN TblInstruction i ON fd.InstructionId = i.Id
                        order by fd.CreatedAt desc
                        ";

                var result = await _context.ExecuteSqlQueryAsync<FavouriteDrugTempVM>(query);
                return result;
                //return await _context.GetTableRowsAsync<TblFavouriteDrugTemplate>("TblFavouriteDrugTemplate");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> GetDataById(string Id)
        {
            try
            {
                var sql = $@"Select * From TblFavouriteDrugTemplate Where Id = '{Id}'";
                var data = await _context.ExecuteSqlQueryAsync<TblFavouriteDrugTemplate>(sql);
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
                var sql = $"DELETE FROM TblFavouriteDrugTemplate WHERE Id ='{id}'";
                await _context.ExecuteSqlQueryAsync<TblRefferTemplate>(sql);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<dynamic> SaveFavouriteDrugTemp(TblFavouriteDrugTemplate TblFavouriteDrugTemplate)
        {
            try
            {
                var saveTemplate = await _context.CreateAsync<TblFavouriteDrugTemplate>(TblFavouriteDrugTemplate);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
