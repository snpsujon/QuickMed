﻿using QuickMed.DB;
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
                var query = @"
              SELECT 
            p.Id,
            p.PrescriptionCode,
            p.PrescriptionDate,
            pp.Id AS PatientId,
            pp.Name as PatientName,
            pp.Mobile,
            pp.Address
        FROM TblPrescription p
        LEFT JOIN TblPatient pp 
            ON p.PatientId = pp.Id";

                var result = await _context.ExecuteSqlQueryAsync<PrescriptionVM>(query);
                return result.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> GetCCList()
        {
            try
            {
                var sql = $@"SELECT Id as value,Name as text FROM TblCCTemplate";
                var cc = await _context.ExecuteSqlQueryAsync<SelectVM>(sql);
                return cc;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<dynamic> GetDXList()
        {
            try
            {
                var sql = $@"SELECT Id as value,Name as text FROM TblDXTemplate";
                var cc = await _context.ExecuteSqlQueryAsync<SelectVM>(sql);
                return cc;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<dynamic> GetDurationsList()
        {
            try
            {
                var sql = $@"SELECT Id as value,Name as text FROM TblDuration";
                var cc = await _context.ExecuteSqlQueryAsync<SelectVM>(sql);
                return cc;
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

        public async Task<dynamic> SavePresCC(List<TblPres_Cc> datas)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblPres_Cc>(datas);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<dynamic> SavePresMH(List<TblPres_MH> datas)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblPres_MH>(datas);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<dynamic> SavePresHO(TblPres_Ho data)
        {
            try
            {
                var saveDetails = await _context.CreateAsync<TblPres_Ho>(data);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> SavePresDH(List<TblPres_DH> datas)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblPres_DH>(datas);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> SavePresOE(List<TblPres_OE> datas)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblPres_OE>(datas);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> SavePresDX(List<TblPres_DX> datas)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblPres_DX>(datas);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> SavePatientReport(List<TblPatientReport> datas)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblPatientReport>(datas);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> SavePrescriptionDetails(List<TblPrescriptionDetails> datas)
        {
            try
            {
                var saveDetails = await _context.CreateMultipleAsync<TblPrescriptionDetails>(datas);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> SavePrescription(TblPrescription data)
        {
            try
            {
                var saveDetails = await _context.CreateAsync<TblPrescription>(data);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> GetPorPResult(string input, bool isMobile)
        {
            try
            {
                var sql = "";
                if (isMobile)
                {
                    sql = $@"SELECT * FROM TblPatient WHERE Mobile = '{input}'";
                    var data = await _context.ExecuteSqlQueryAsync<TblPatient>(sql);
                    return data.LastOrDefault();
                }
                else
                {
                    sql = $@"SELECT *  FROM TblPrescription WHERE PrescriptionCode = '{input}'";
                    var data = await _context.ExecuteSqlQueryAsync<TblPrescription>(sql);
                    return data.FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }



        }
    }
}
