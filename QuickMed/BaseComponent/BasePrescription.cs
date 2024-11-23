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
                var result = await JS.InvokeAsync<object>("getPresData");
                if (result is not null)
                {
                    //var jsonString = result.ToString();
                    //var treatments = JsonSerializer.Deserialize<List<TreatmentPopVM>>(jsonString);
                    //var prescription = new TblPrescription()
                    //{
                    //    Id = Guid.NewGuid(),
                    //    PatientId = patient.Id,
                    //    Date = DateTime.Now,
                    //    Dx = "Dx",
                    //    CC = "CC",
                    //    NextMeetingDate = NextMeetingDate,
                    //    PrescriptionDetails = treatments.Select(x => new TblPrescriptionDetails()
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        PrescriptionId = Guid.NewGuid(),
                    //        BrandId = Guid.Parse(x.brand.value),
                    //        DoseId = Guid.Parse(x.dose.value),
                    //        DurationId = Guid.Parse(x.duration.value),
                    //        InstructionId = Guid.Parse(x.instruction.value)
                    //    }).ToList()
                    //};
                    //await App.Database.InsertAsync(prescription);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
