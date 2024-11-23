using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
    public class BasePrescription : ComponentBase
    {
        [Inject]
        public IPrescription _pres { get; set; }
        [Inject]
        public IMixTemp _mix { get; set; }



        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public ITeatmentTemp _teatmentTemp { get; set; }
        [Inject]
        public IAdvice _adviceTemp { get; set; }


        public TblPatient patient = new();
        public DrugMedicine brnad = new();
        public TblDose dose = new();
        public IEnumerable<TblPatient>? patients { get; set; }
        public IEnumerable<DrugMedicine>? brands { get; set; }
        public IEnumerable<TblDose>? doses { get; set; }
        public IEnumerable<TblTreatmentTemplate>? treatmentTemps { get; set; }

        public IEnumerable<TblFavouriteDrugTemplate>? FavDrugTemps { get; set; }

        public IEnumerable<TblPerceptionTemplate>? PrescTemps { get; set; }

        public IEnumerable<TblBrandTemplate>? BrandTemps { get; set; }

        public IEnumerable<TblDoseTemplate>? DoseTemps { get; set; }

        public IEnumerable<TblInstructionTemplate> InsTemps { get; set; }

        public IEnumerable<TblDurationTemplate> DuraTionTemps { get; set; }

        //public IEnumerable<TblAdviceTemplate> adviceMasters { get; set; }

        public IEnumerable<TblRefferTemplate> RefferTemps { get; set; }

        public string tblid { get; set; }

        public DotNetObjectReference<BasePrescription> ObjectReference { get; private set; }

        public List<DrugMedicine> Brands = new List<DrugMedicine>();
        public List<TblDose> Dose = new List<TblDose>();
        public List<TblDuration> Duration = new List<TblDuration>();
        public List<TblInstruction> Instructions = new List<TblInstruction>();
        public List<TblAdviceTemplateDetails> adviceDetails = new List<TblAdviceTemplateDetails>();
        public List<TblAdviceTemplate> adviceMasters = new List<TblAdviceTemplate>();
        public List<TblNotesTemplate> notesMasters = new List<TblNotesTemplate>();
        public List<TblNotesTempDetails> noteDetails = new List<TblNotesTempDetails>();
        public List<TblIXTemplate> ixMasters = new List<TblIXTemplate>();
        public List<TblIXDetails> ixDetails = new List<TblIXDetails>();
        public List<TblPres_Cc> tblPres_Ccs = new List<TblPres_Cc>();
        public List<TblPres_MH> tblPres_MHs = new List<TblPres_MH>();

        public TblAdviceTemplate adviceTemplate = new();

        public int SelectedDays { get; set; } = 0;
        public DateTime NextMeetingDate { get; set; } = DateTime.Now;


        private BaseFavouriteDrugTemp _favdrag { get; set; } = new BaseFavouriteDrugTemp();
        public DotNetObjectReference<BaseFavouriteDrugTemp> ObjectReferenceForFavDrag { get; private set; }

        [Inject]
        public IServiceProvider ServiceProvider { get; set; }


        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            ObjectReferenceForFavDrag = DotNetObjectReference.Create(_favdrag);
            treatmentTemps = await App.Database.GetTableRowsAsync<TblTreatmentTemplate>("TblTreatmentTemplate");
            FavDrugTemps = await App.Database.GetTableRowsAsync<TblFavouriteDrugTemplate>("TblFavouriteDrugTemplate");
            PrescTemps = await App.Database.GetTableRowsAsync<TblPerceptionTemplate>("TblPerceptionTemplate");
            adviceMasters = new();
            adviceMasters = await App.Database.GetTableRowsAsync<TblAdviceTemplate>("TblAdviceTemplate");
            RefferTemps = await App.Database.GetTableRowsAsync<TblRefferTemplate>("TblRefferTemplate");
            Brands = new();
            Brands = await _mix.GetAllMedicine();
            Dose = new();
            Dose = await App.Database.GetTableRowsAsync<TblDose>("TblDose");
            Duration = new();
            Duration = await App.Database.GetTableRowsAsync<TblDuration>("TblDuration");
            Instructions = new();
            Instructions = await App.Database.GetTableRowsAsync<TblInstruction>("TblInstruction");
            notesMasters = new();
            notesMasters = await App.Database.GetTableRowsAsync<TblNotesTemplate>("TblNotesTemplate");
            ixMasters = new();
            ixMasters = await App.Database.GetTableRowsAsync<TblIXTemplate>("TblIXTemplate");

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
                await InitializeJS();
            }
        }

        protected async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        }

        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            await JS.InvokeVoidAsync("setInstanceReferenceForFavDrag", ObjectReferenceForFavDrag);
            await JS.InvokeVoidAsync("makeSelect2", true);
            await JS.InvokeVoidAsync("makeSelect2Custom", "select2C", "GetMedicines", 3);
            await JS.InvokeVoidAsync("setupEditableTable", "TretmentTmpAdviceTbl", "add_Advice");
            await JS.InvokeVoidAsync("setupEditableTable", "refferTable", "but_reffer");
            await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");
            await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpAdviceTbl");
            await JS.InvokeVoidAsync("MakeAvro", "avrotranslate");
            await JS.InvokeVoidAsync("OnChangeEvent", "adviceSelect", "AdviceChange", ObjectReference);
            await JS.InvokeVoidAsync("OnChangeEvent", "presRefferTempSelect", "RefferChange", ObjectReference);
            await JS.InvokeVoidAsync("OnChangeEvent", "nxtMeetDateSelect", "NextMeetDateChange", ObjectReference);
            await JS.InvokeVoidAsync("OnChangeEvent", "presDrugTempSelect", "LoadFavDrugTemplate", ObjectReference);
            await JS.InvokeVoidAsync("OnChangeEvent", "presTreatTempSelect", "LoadTreatMentTemplate", ObjectReference);
            await JS.InvokeVoidAsync("OnChangeEvent", "ixTempSelect", "IxChange", ObjectReference);
            await JS.InvokeVoidAsync("OnChangeEvent", "noteTempSelect", "NotesChange", ObjectReference);


            await JS.InvokeVoidAsync("initializeQuill", "#editors_pres");

            //await JS.InvokeVoidAsync("clearQuillContent", "#editors_pres");


            //await JS.InvokeVoidAsync("setupEditableTable", "MixTempTbl", "add_MixTemp");
            //await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");

        }
        public void Dispose()
        {
            ObjectReference?.Dispose();
        }

        [JSInvokable("GetMedicines")]
        public Task<List<DrugMedicine>> LoadMedicines(string search)
        {
            var filtered = string.IsNullOrEmpty(search)
                ? Brands
                : Brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return Task.FromResult(filtered);
        }


        public async Task OsudAddbtn()
        {
            try
            {
                var result = await JS.InvokeAsync<object>("OsudAddbtn");
                if (result is not null)
                {
                    var jsonString = result.ToString();

                    var treatments = JsonSerializer.Deserialize<List<TreatmentPopVM>>(jsonString);
                    await JS.InvokeVoidAsync("populateTreatmentTable", treatments, "TretmentTmpTbl");

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [JSInvokable]
        public async Task AdviceChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    adviceDetails = await _teatmentTemp.GetAdviceDataById(selectedData);
                    await JS.InvokeVoidAsync("populateAdviceTable", adviceDetails, "TretmentTmpAdviceTbl");

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [JSInvokable]
        public async Task RefferChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    await JS.InvokeVoidAsync("setQuillContent", "#editors_pres", RefferTemps.FirstOrDefault(x => x.Id == Guid.Parse(selectedData)).Details);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [JSInvokable]
        public async Task IxChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    ixDetails = await _teatmentTemp.GetIXDataById(selectedData);
                    await JS.InvokeVoidAsync("populateIXTablePres", ixDetails, "TretmentTmpIXTbl");

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [JSInvokable]
        public async Task NotesChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    noteDetails = await _teatmentTemp.GetNoteDataById(selectedData);
                    await JS.InvokeVoidAsync("populateNoteTablePres", noteDetails, "TretmentTmpNotesTbl");

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [JSInvokable]
        public async Task NextMeetDateChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    await JS.InvokeVoidAsync("changeNxtDatebyVal", selectedData);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task OnSelectedDaysChanged()
        {
            string value = "2";
            if (int.TryParse(value, out var days))
            {
                SelectedDays = days;
                NextMeetingDate = DateTime.Now.AddDays(SelectedDays);
            }
            else
            {
                SelectedDays = 0;
                NextMeetingDate = DateTime.Now;
            }
            StateHasChanged();
        }

        public async Task AddaRow(string tblid, bool isSelect = false, string functionName = "", string className = "")
        {
            await JS.InvokeVoidAsync("myrowAddNew", tblid, isSelect, functionName, className);
        }


        public async Task UpdateBMI()
        {
            await JS.InvokeVoidAsync("updateBMI");
        }


        [JSInvokable("GetOusudData")]
        public async Task<dynamic> GetOusudData(string ousudData)
        {
            var sameGen = await _teatmentTemp.GetBrandsSameGenaric(ousudData);

            var result = new
            {
                Ousud = sameGen,
                Dose = Dose.Select(x => new SelectVM
                {
                    text = x.Name,
                    value = x.Id
                }).ToList(),
                Duration = Duration.Select(x => new SelectVM
                {
                    text = x.Name,
                    value = x.Id
                }).ToList(),
                Instruction = Instructions.Select(x => new SelectVM
                {
                    text = x.Name,
                    value = x.Id
                }).ToList()
            };
            return result;

            // Example data
            //return new List<string> { "Medicine 1", "Medicine 2", "Medicine 3" };
        }


        [JSInvokable("LoadTreatMentTemplate")]
        public async Task LoadTreatMentTemplate(string selectedData)
        {
            try
            {
                if (selectedData is not null)
                {

                    List<FavouriteDrugTempVM> treatments = await _pres.TblTreatmentTempDetails(Guid.Parse(selectedData));
                    List<TreatmentPopVM> treatmentPopVMs = new List<TreatmentPopVM>();
                    var result = new object();
                    if (treatments.Count() > 0)
                    {
                        //Load the treatment template to the prescription table
                        foreach (var treatment in treatments)
                        {
                            TreatmentPopVM treatmentPopVM = new TreatmentPopVM()
                            {
                                brand = new BrandVM()
                                {
                                    text = treatment.BrandName,
                                    value = treatment.BrandId.ToString()
                                },
                                dose = new DoseVM()
                                {
                                    text = treatment.DoseName,
                                    value = treatment.DoseId.ToString()
                                },
                                duration = new DurationVM()
                                {
                                    text = treatment.DurationName,
                                    value = treatment.DurationId.ToString()
                                },
                                instruction = new InstructionVM()
                                {
                                    text = treatment.InstructionName,
                                    value = treatment.InstructionId.ToString()
                                }
                            };
                            treatmentPopVMs.Add(treatmentPopVM);
                            result = await JS.InvokeAsync<object>("pushtoPrescription", treatmentPopVM);
                        }
                        if (result is not null)
                        {
                            var jsonString = result.ToString();
                            var treatments1 = JsonSerializer.Deserialize<List<TreatmentPopVM>>(jsonString);
                            await JS.InvokeVoidAsync("populateTreatmentTable", treatments1, "TretmentTmpTbl");
                        }

                        adviceDetails = new();
                        //Load the advice template to the prescription table
                        adviceDetails = await _teatmentTemp.GetAdviceDataById(treatments.FirstOrDefault().Id.ToString());
                        await JS.InvokeVoidAsync("populateAdviceTable", adviceDetails, "TretmentTmpAdviceTbl");


                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [JSInvokable("LoadFavDrugTemplate")]
        public async Task LoadFavDrugTemplate(string selectedData)
        {
            try
            {
                if (selectedData is not null)
                {
                    FavouriteDrugTempVM favDrugTemp = await _pres.GetFavDrugbyId(Guid.Parse(selectedData));
                    TreatmentPopVM treatmentPopVM = new TreatmentPopVM()
                    {
                        brand = new BrandVM()
                        {
                            text = favDrugTemp.BrandName,
                            value = favDrugTemp.BrandId.ToString()
                        },
                        dose = new DoseVM()
                        {
                            text = favDrugTemp.DoseName,
                            value = favDrugTemp.DoseId.ToString()
                        },
                        duration = new DurationVM()
                        {
                            text = favDrugTemp.DurationName,
                            value = favDrugTemp.DurationId.ToString()
                        },
                        instruction = new InstructionVM()
                        {
                            text = favDrugTemp.InstructionName,
                            value = favDrugTemp.InstructionId.ToString()
                        }
                    };
                    var result = await JS.InvokeAsync<object>("pushtoPrescription", treatmentPopVM);
                    if (result is not null)
                    {
                        var jsonString = result.ToString();
                        var treatments = JsonSerializer.Deserialize<List<TreatmentPopVM>>(jsonString);
                        await JS.InvokeVoidAsync("populateTreatmentTable", treatments, "TretmentTmpTbl");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [JSInvokable("GetCCSelectData")]
        public async Task<dynamic> GetCCSelectData()
        {
            var CC = await _pres.GetCCList();
            var duration = await _pres.GetDurationsList();
            var DM = new List<object>
            {
                new
                {
                    text = "দিন",
                    value = "Day"
                },
                new
                {
                    text = "সপ্তাহ",
                    value = "Week"
                },
                new
                {
                    text = "মাস",
                    value = "Month"
                },
                new
                {
                    text = "বছর",
                    value = "Year"
                }
            };

            var returndata = new
            {
                ccList = CC,
                duration = duration,
                dM = DM
            };
            return returndata;
        }

        [JSInvokable("GetDHSelectData")]
        public async Task<dynamic> GetDHSelectData()
        {
            var dh = new
            {
                Brands = Brands.Select(x => new SelectVM
                {
                    text = x.Name,
                    value = x.Id
                }).ToList(),
            };
            return dh;
        }
        [JSInvokable("GetDXSelectData")]
        public async Task<dynamic> GetDXSelectData()
        {
            var dx = await _pres.GetCCList();
            var dh = new
            {
                dx = dx
            };
            return dh;
        }


        public async Task SaveOnly()
        {
            try
            {
                var result = await JS.InvokeAsync<JsonElement>("getPresData");


                var PatientId = Guid.NewGuid();
                var AdviceId = Guid.NewGuid();
                var IxId = Guid.NewGuid();
                var PresId = Guid.NewGuid();
                var regNo = "";


                if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
                {
                    if (result.TryGetProperty("pdata", out JsonElement pt))
                    {
                        regNo = pt.GetProperty("regNo").GetString();
                        var patientInfo = new TblPatient()
                        {
                            Id = PatientId,
                            Name = pt.GetProperty("name").GetString(),
                            Age = pt.GetProperty("age").GetString(),
                            Gender = pt.GetProperty("sex").GetString(),
                            Address = pt.GetProperty("address").GetString(),
                            Mobile = pt.GetProperty("mobile").GetString(),
                            Weight = pt.GetProperty("weight").GetDecimal(),
                            AdmissionDate = pt.GetProperty("date").GetDateTime(),
                            HeightInch = pt.GetProperty("height").GetDecimal()
                        };

                    }
                    if (result.TryGetProperty("advice", out JsonElement adviceArrayElement))
                    {
                        if (adviceArrayElement.ValueKind == JsonValueKind.Array)
                        {
                            var adviceList = adviceArrayElement.EnumerateArray()
                                .Select(item => item.GetString())
                                .ToList();
                            if (adviceList.Count() > 0)
                            {
                                adviceTemplate = new TblAdviceTemplate
                                {
                                    Id = AdviceId,
                                    AdviceTemplateName = regNo + "_Advice"
                                };
                                await _adviceTemp.SaveAdviceTemplate(adviceTemplate);
                                adviceDetails = new List<TblAdviceTemplateDetails>();
                                foreach (var advice in adviceList)
                                {
                                    adviceDetails.Add(new TblAdviceTemplateDetails
                                    {
                                        Id = Guid.NewGuid(),
                                        AdviceTemplateId = AdviceId,
                                        Advice = advice
                                    });
                                }
                                await _adviceTemp.SaveAdviceTemplateDetails(adviceDetails);
                            }

                        }
                    }

                    if (result.TryGetProperty("ccTableData", out JsonElement ccTableDataArray))
                    {
                        var CCdataList = ccTableDataArray.EnumerateArray()
                                .Select(item => new
                                {
                                    cc = item.GetProperty("column_2").GetString(),
                                    duration = item.GetProperty("column_3").GetString(),
                                    dm = item.GetProperty("column_4").GetString(),
                                }).ToList();
                        tblPres_Ccs = new List<TblPres_Cc>();
                        foreach (var cc in CCdataList)
                        {
                            tblPres_Ccs.Add(new TblPres_Cc
                            {
                                Id = Guid.NewGuid(),
                                Pres_ID = PresId,
                                CcName = cc.cc,
                                DurationId = Guid.Parse(cc.duration),
                                Dm_Id = cc.dm
                            });
                        }
                    }

                    if (result.TryGetProperty("hoTableData", out JsonElement hoTableData))
                    {
                        var hoTableDatas = JsonSerializer.Deserialize<TblPres_Ho>(hoTableData.ToString());
                        hoTableDatas.Id = Guid.NewGuid();
                        hoTableDatas.Pres_ID = PresId;
                    }
                    if (result.TryGetProperty("mhTableData", out JsonElement mhTableData))
                    {
                        tblPres_MHs = new List<TblPres_MH>();
                        foreach (JsonElement element in mhTableData.EnumerateArray())
                        {
                            foreach (var property in element.EnumerateObject())
                            {

                                tblPres_MHs.Add(new TblPres_MH
                                {
                                    Id = Guid.NewGuid(),
                                    Pres_ID = PresId,
                                    MH = property.Name,
                                    Value = property.Value.GetString()
                                });
                            }
                        }

                    }
                    if (result.TryGetProperty("oeTableData", out JsonElement oeTableData))
                    {
                        List<TblPres_OE> tblPres_OEs = new List<TblPres_OE>();
                        foreach (JsonElement element in oeTableData.EnumerateArray())
                        {
                            foreach (var property in element.EnumerateObject())
                            {
                                string key = property.Name; // Extract the key (e.g., BP, Pulse)
                                JsonElement valueElement = property.Value;

                                if (valueElement.TryGetProperty("value", out JsonElement value) &&
                                    valueElement.TryGetProperty("unit", out JsonElement unit))
                                {
                                    tblPres_OEs.Add(new TblPres_OE
                                    {
                                        Id = Guid.NewGuid(),
                                        Pres_ID = PresId,
                                        Name = key,
                                        value = value.GetString(),
                                        Unit = unit.GetString()
                                    });
                                }
                            }
                        }
                    }

                    if (result.TryGetProperty("dhTableData", out JsonElement dhTableData))
                    {
                        var dhdataList = dhTableData.EnumerateArray()
                                .Select(item => new
                                {
                                    dh = item.GetProperty("column_2").GetString()
                                }).ToList();
                        List<TblPres_DH> tblPres_DHs = new List<TblPres_DH>();
                        foreach (var cc in dhdataList)
                        {
                            tblPres_DHs.Add(new TblPres_DH
                            {
                                Id = Guid.NewGuid(),
                                Pres_ID = PresId,
                                BrandID = Guid.Parse(cc.dh)
                            });
                        }
                    }

                    if (result.TryGetProperty("dxTableData", out JsonElement dxTableData))
                    {
                        var dXdataList = dxTableData.EnumerateArray()
                                .Select(item => new
                                {
                                    dX = item.GetProperty("column_2").GetString()
                                }).ToList();
                        List<TblPres_DX> ListData = new List<TblPres_DX>();
                        foreach (var cc in dXdataList)
                        {
                            ListData.Add(new TblPres_DX
                            {
                                Id = Guid.NewGuid(),
                                Pres_ID = PresId,
                                DxTempId = Guid.Parse(cc.dX)
                            });
                        }
                    }

                    if (result.TryGetProperty("ixTableData", out JsonElement ixTableData))
                    {
                        if (ixTableData.ValueKind == JsonValueKind.Array)
                        {
                            var ixList = ixTableData.EnumerateArray()
                                .Select(item => new
                                {
                                    ix = item.GetProperty("column_2").GetString()
                                }).ToList();
                            if (ixList.Count() > 0)
                            {
                                TblIXTemplate ixTemplate = new TblIXTemplate
                                {
                                    Id = IxId,
                                    TemplateName = regNo + "_IX"
                                };
                                List<TblIXDetails> ixDetails = new List<TblIXDetails>();
                                foreach (var item in ixList)
                                {
                                    ixDetails.Add(new TblIXDetails
                                    {
                                        Id = Guid.NewGuid(),
                                        TblIXTempMasterId = IxId,
                                        Name = item.ix

                                    });
                                }
                            }

                        }
                    }

                    if (result.TryGetProperty("noteTableData", out JsonElement noteTableData))
                    {
                        if (noteTableData.ValueKind == JsonValueKind.Array)
                        {
                            var noteList = noteTableData.EnumerateArray()
                                .Select(item => new
                                {
                                    note = item.GetProperty("column_2").GetString()
                                }).ToList();
                            if (noteList.Count() > 0)
                            {
                                TblNotesTemplate notesTemplate = new TblNotesTemplate
                                {
                                    Id = Guid.NewGuid(),
                                    Name = regNo + "_Notes"
                                };
                                List<TblNotesTempDetails> noteDetails = new List<TblNotesTempDetails>();
                                foreach (var item in noteList)
                                {
                                    noteDetails.Add(new TblNotesTempDetails
                                    {
                                        Id = Guid.NewGuid(),
                                        TblNotesTempMasterId = notesTemplate.Id,
                                        Name = item.note

                                    });
                                }
                            }

                        }
                    }

                    if (result.TryGetProperty("reportTableData", out JsonElement reportTableData))
                    {
                        if (reportTableData.ValueKind == JsonValueKind.Array)
                        {
                            var presList = reportTableData.EnumerateArray()
                                .Select(item => new
                                {
                                    date = item.GetProperty("column_2").GetDateTime(),
                                    rptName = item.GetProperty("column_3").GetString(),
                                    res = item.GetProperty("column_4").GetString(),
                                    unit = item.GetProperty("column_5").GetString()
                                }).ToList();

                            List<TblPatientReport> tblPatientReports = new List<TblPatientReport>();
                            foreach (var item in presList)
                            {
                                tblPatientReports.Add(new TblPatientReport
                                {
                                    Id = Guid.NewGuid(),
                                    PatientId = PatientId,
                                    PresId = PresId,
                                    ReportName = item.rptName,
                                    ReportDate = item.date,
                                    Result = item.res,
                                    Unit = item.unit

                                });

                            }

                        }
                    }

                    if (result.TryGetProperty("treatment", out JsonElement treatmentArrayElement))
                    {
                        if (treatmentArrayElement.ValueKind == JsonValueKind.Array)
                        {
                            var treatmentList = treatmentArrayElement.EnumerateArray()
                                .Select(item => new
                                {
                                    Index = item.GetProperty("index").GetInt32(),
                                    Brand = item.GetProperty("brand").GetProperty("value").GetString(),
                                    Dose = item.GetProperty("dose").GetProperty("value").GetString(),
                                    Duration = item.GetProperty("duration").GetProperty("value").GetString(),
                                    Instruction = item.GetProperty("instruction").GetProperty("value").GetString()
                                }).ToList();
                            if (treatmentList.Count() > 0)
                            {
                                List<TblPrescriptionDetails> rxDetails = new List<TblPrescriptionDetails>();
                                foreach (var item in treatmentList)
                                {
                                    rxDetails.Add(new TblPrescriptionDetails
                                    {
                                        Id = Guid.NewGuid(),
                                        Brand = Guid.Parse(item.Brand),
                                        Dose = Guid.Parse(item.Dose),
                                        Duration = Guid.Parse(item.Duration),
                                        Instruction = Guid.Parse(item.Instruction),
                                        PrescriptionMasterId = PresId
                                    });
                                }
                                //await _teatmentTemp.SaveTreatmentTempDetails(templateDetails);
                            }
                        }
                    }

                    if (result.TryGetProperty("treatment", out JsonElement pres))
                    {
                        var LicenceKey = await App.Database.GetTableRowsAsync<TblLicenseInfo>("TblLicenseInfo");
                        var key = "";
                        if (LicenceKey != null)
                        {
                            key = LicenceKey.FirstOrDefault().LicenseKey;
                        }
                        var PresInfo = new TblPrescription
                        {
                            Id = PresId,
                            PatientId = PatientId,
                            AdviceId = AdviceId,
                            IxId = IxId,
                            PrescriptionDate = DateTime.Now,
                            NextMeetingDate = result.GetProperty("nxtMeetData").GetProperty("nextMeetingDate").GetDateTime(),
                            NextMeetingValue = result.GetProperty("nxtMeetData").GetProperty("nextMeetingDuration").GetInt32(),
                            Payment = result.GetProperty("nxtMeetData").GetProperty("payment").GetInt32(),
                            RefferedBy = result.GetProperty("nxtMeetData").GetProperty("refferedBy").GetString(),
                            IsSynced = false,
                            RefferedTo = result.GetProperty("reffer").GetString(),
                            Height = result.GetProperty("pdata").GetProperty("height").GetInt32(),
                            weight = result.GetProperty("pdata").GetProperty("bmiweight").GetInt32(),
                            PrescriptionCode = regNo,
                            CreatedBy = key,
                            CreatedAt = DateTime.Now,
                            LicenseKey = key
                        };
                    }






                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
