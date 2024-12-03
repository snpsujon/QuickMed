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

        public async Task<dynamic> DeleteAsync(string id)
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

        public async Task<dynamic> GetAll(PrescriptionFilterParameters filterParams)
        {
            try
            {




                var query = $@"
            
                            SELECT 
                                p.Id,
                                p.Id as PrescriptioId,
                                p.PrescriptionCode,
                                p.PrescriptionDate,
                                pp.Id AS PatientId,
                                pp.Name AS PatientName,
                                pp.Mobile AS MobileNumber,
                                pp.Address,
                                GROUP_CONCAT(nt.Name, ', ') AS Plan,
                                GROUP_CONCAT(dx.Name, ', ') AS Dx
                            FROM TblPrescription p
                            LEFT JOIN TblPatient pp ON p.PatientId = pp.Id
                            LEFT JOIN TblPres_Note ntp ON ntp.Pres_ID = p.Id
                            LEFT JOIN TblNotesTemplate nt ON nt.Id = ntp.NoteTempId
                            LEFT JOIN TblPres_DX dxp ON dxp.Pres_ID = p.Id
                            LEFT JOIN TblDXTemplate dx ON dx.Id = dxp.DxTempId
                            LEFT JOIN TblPrescriptionDetails tpd on tpd.PrescriptionMasterId = p.Id
                            WHERE 
                                (
                                    ('{filterParams.Mobile}' = '' OR '{filterParams.Mobile}' IS NULL OR pp.Mobile = '{filterParams.Mobile}') AND
                                    ('{filterParams.PrescriptionCode}' = '' OR '{filterParams.PrescriptionCode}' IS NULL OR p.PrescriptionCode = '{filterParams.PrescriptionCode}') AND
		                            (dxp.DxTempId = '{filterParams.DxTempId}' OR '{filterParams.DxTempId}' = '' OR '{filterParams.DxTempId}' IS NULL) AND
		                             (p.PrescriptionDate BETWEEN '{filterParams.StartDate?.Ticks}' AND '{filterParams.EndDate?.Ticks}' OR ('{filterParams.StartDate?.Ticks}' IS NULL AND '{filterParams.EndDate?.Ticks}' IS NULL)) AND
                                    (tpd.Brand = '{filterParams.BrandId}' OR '{filterParams.BrandId}' = '' OR '{filterParams.BrandId}' IS NULL)
		
                                ) 
	
	
                            GROUP BY 
                                p.Id, 
                                p.PrescriptionCode, 
                                p.PrescriptionDate, 
                                pp.Id, 
                                pp.Name, 
                                pp.Mobile, 
                                pp.Address;
        ";


                // Pass the DynamicParameters to the ExecuteSqlWithParamQueryAsync method


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
                        d.Id as DurationId,d.Name as DurationName,i.Id as InstructionId,i.Name as InstructionName, ttm.Id as TempId , ttm.Name
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
                    PrescriptionDetailsVM pres = new PrescriptionDetailsVM();

                    sql = $@"SELECT * FROM TblPrescription 
                          WHERE PrescriptionCode = '{input}'";
                    pres.tblPrescription = (await _context.ExecuteSqlQueryAsync<TblPrescription>(sql)).FirstOrDefault();

                    sql = $@"SELECT pat.* FROM TblPatient pat
                           INNER JOIN TblPrescription p ON p.PatientId = pat.Id 
                           WHERE PrescriptionCode = '{input}'";
                    pres.tblPatient = (await _context.ExecuteSqlQueryAsync<TblPatient>(sql)).FirstOrDefault();

                    sql = $@"SELECT h.* FROM TblPres_Ho h
                            JOIN TblPrescription p ON p.Id = h.Pres_ID
                            WHERE PrescriptionCode = '{input}'";
                    pres.tblPres_Ho = (await _context.ExecuteSqlQueryAsync<TblPres_Ho>(sql)).FirstOrDefault();




                    sql = $@"SELECT s.* FROM TblAdviceTemplate a
                            LEFT JOIN TblAdviceTemplateDetails s ON s.AdviceTemplateId = a.Id
                            INNER JOIN TblPrescription p ON p.AdviceId = a.Id 
                            WHERE PrescriptionCode = '{input}'";
                    pres.tblAdviceTemplateDetails = (await _context.ExecuteSqlQueryAsync<TblAdviceTemplateDetails>(sql)).ToList();


                    sql = $@"SELECT nd.* FROM TblNotesTemplate n
                            LEFT JOIN TblNotesTempDetails nd ON nd.TblNotesTempMasterId = n.Id
                            INNER JOIN TblPrescription p ON p.NoteId = n.Id 
                            WHERE PrescriptionCode = '{input}'";
                    pres.noteDetails = (await _context.ExecuteSqlQueryAsync<TblNotesTempDetails>(sql)).ToList();


                    sql = $@"SELECT ixd.* FROM TblIXTemplate i
                            LEFT JOIN TblIXDetails ixd ON ixd.TblIXTempMasterId = i.Id
                            INNER JOIN TblPrescription p ON p.IxId = i.Id 
                            WHERE PrescriptionCode = '{input}'";
                    pres.ixDetails = (await _context.ExecuteSqlQueryAsync<TblIXDetails>(sql)).ToList();


                    sql = $@"SELECT CC.* FROM TblPres_Cc CC
                            JOIN TblPrescription p ON p.Id = CC.Pres_ID 
                            WHERE PrescriptionCode = '{input}'";
                    pres.tblPres_Ccs = (await _context.ExecuteSqlQueryAsync<TblPres_Cc>(sql)).ToList();


                    sql = $@"SELECT m.* FROM TblPres_MH m
                            JOIN TblPrescription p ON p.Id = m.Pres_ID 
                            WHERE PrescriptionCode = '{input}'";
                    pres.tblPres_MHs = (await _context.ExecuteSqlQueryAsync<TblPres_MH>(sql)).ToList();


                    sql = $@"SELECT o.* FROM TblPres_OE o 
                            JOIN TblPrescription p ON p.Id = o.Pres_ID 
                            WHERE PrescriptionCode = '{input}'";
                    pres.tblPres_OEs = (await _context.ExecuteSqlQueryAsync<TblPres_OE>(sql)).ToList();

                    sql = $@"SELECT d.* FROM TblPres_DX d
                            JOIN TblPrescription p ON p.Id = d.Pres_ID 
                            WHERE PrescriptionCode = '{input}'";
                    pres.tblPres_DXes = (await _context.ExecuteSqlQueryAsync<TblPres_DX>(sql)).ToList();


                    sql = $@"SELECT r.* FROM TblPatientReport r
                            JOIN TblPrescription p ON p.Id = r.PresId 
                            WHERE PrescriptionCode = '{input}'";
                    pres.tblPatientReports = (await _context.ExecuteSqlQueryAsync<TblPatientReport>(sql)).ToList();

                    sql = $@"SELECT r.* FROM TblPres_DH r
                            JOIN TblPrescription p ON p.Id = r.Pres_ID 
                            WHERE PrescriptionCode = '{input}'";
                    pres.tblPres_DHs = (await _context.ExecuteSqlQueryAsync<TblPres_DH>(sql)).ToList();




                    sql = $@"select
                        p.AdviceId as Id,dm.Id as BrandId,
                        ddd.Name || ' - ' || dm.Name || ' ( ' || dm.Strength || ' ) ' || ' - ' || dg.Name as BrandName,
                        dd.Id as DoseId,dd.Name as DoseName,
                        d.Id as DurationId,d.Name as DurationName,i.Id as InstructionId,i.Name as InstructionName, p.Id as TempId 
                        from
                        TblPrescriptionDetails ttd
                        JOIN TblPrescription p on ttd.PrescriptionMasterId = p.Id
                        LEFT JOIN DrugMedicine dm on ttd.Brand = dm.Id
                        LEFT JOIN DrugDosage ddd ON dm.DosageId = ddd.Id
                        LEFT JOIN DrugGeneric dg ON dm.GenericId = dg.Id
                        LEFT JOIN DrugManufacturer c ON dm.ManufacturerId = c.Id
                        LEFT JOIN TblDose dd ON ttd.Dose = dd.Id
                        LEFT JOIN TblDuration d ON ttd.Duration = d.Id
                        LEFT JOIN TblInstruction i ON ttd.Instruction = i.Id
						WHERE p.PrescriptionCode = '{input}'";
                    pres.rxDetails = (await _context.ExecuteSqlQueryAsync<FavouriteDrugTempVM>(sql)).ToList();


                    return pres;
                }
            }
            catch (Exception)
            {

                throw;
            }



        }
    }
}
