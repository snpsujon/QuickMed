using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
    public class BaseTreatmentTemp : ComponentBase
    {

        [Inject]
        public IMixTemp _mix { get; set; }

        [Inject]
        public ITeatmentTemp _teatmentTemp { get; set; }
        [Inject]
        public IAdvice _adviceTemp { get; set; }
        public DotNetObjectReference<BaseTreatmentTemp> ObjectReference { get; private set; }

        public TblTreatmentTemplate treatmentTemplate = new();
        public List<TblTreatmentTempDetails> templateDetails = new List<TblTreatmentTempDetails>();
        public List<TblTreatmentTemplate> templates = new List<TblTreatmentTemplate>();
        public TblTreatmentTempDetails details = new TblTreatmentTempDetails();
        public TblAdviceTemplate adviceTemplate = new();
        public List<TblAdviceTemplateDetails> adviceDetails = new List<TblAdviceTemplateDetails>();
        public TblAdviceTemplateDetails TblAdviceTemplate = new();
        public List<DrugMedicine> Brands = new List<DrugMedicine>();
        public List<TblDose> Dose = new List<TblDose>();
        public List<TblDuration> Duration = new List<TblDuration>();
        public List<TblInstruction> Instructions = new List<TblInstruction>();
        public List<TblAdviceTemplate> adviceMasters = new List<TblAdviceTemplate>();



        [Inject]
        public IJSRuntime JS { get; set; }


        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);

            //await JS.InvokeVoidAsync("makeSelect2", true);
            Brands = new();
            Brands = await _mix.GetAllMedicine();
            Dose = new();
            Dose = await App.Database.GetTableRowsAsync<TblDose>("TblDose");
            Duration = new();
            Duration = await App.Database.GetTableRowsAsync<TblDuration>("TblDuration");
            Instructions = new();
            Instructions = await App.Database.GetTableRowsAsync<TblInstruction>("TblInstruction");
            adviceMasters = new();
            adviceMasters = await App.Database.GetTableRowsAsync<TblAdviceTemplate>("TblAdviceTemplate");
            templates = new();
            templates = await App.Database.GetTableRowsAsync<TblTreatmentTemplate>("TblTreatmentTemplate");


        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsInvokeAction();
            }
        }

        public async Task JsInvokeAction()
        {
            try
            {
                await JS.InvokeVoidAsync("makeSelect2", true);
                await JS.InvokeVoidAsync("makeSelect2Custom", "GetMedicines", 3);
                await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
                await JS.InvokeVoidAsync("setupEditableTable", "TretmentTmpAdviceTbl", "add_Advice");
                await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");
                await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpAdviceTbl");
                await JS.InvokeVoidAsync("OnChangeEvent", "adviceSelect", "AdviceChange", ObjectReference);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Dispose()
        {
            ObjectReference?.Dispose();
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

        public async Task SaveTemplate()
        {
            try
            {
                var result = await JS.InvokeAsync<JsonElement>("GetTretmentTempData");

                var AdviceId = Guid.NewGuid();
                var TemplateId = Guid.NewGuid();
                string templateName = "";

                if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
                {

                    if (result.TryGetProperty("templateName", out JsonElement templateNameElement))
                    {
                        templateName = templateNameElement.GetString();
                        treatmentTemplate = new();
                        treatmentTemplate = new TblTreatmentTemplate
                        {
                            Id = TemplateId,
                            Name = templateName,
                            AdviceId = AdviceId
                        };
                        await _teatmentTemp.SaveTreatmentTemp(treatmentTemplate);
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
                                    AdviceTemplateName = templateName + "_Advice"
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
                    else
                    {
                        Console.WriteLine("templateName not found.");
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
                                templateDetails = new List<TblTreatmentTempDetails>();
                                foreach (var item in treatmentList)
                                {
                                    templateDetails.Add(new TblTreatmentTempDetails
                                    {
                                        Id = Guid.NewGuid(),
                                        TreatmentTempId = TemplateId.ToString(),
                                        BrandId = item.Brand,
                                        DoseId = item.Dose,
                                        DurationId = item.Duration,
                                        InstructionId = item.Instruction


                                    });
                                }
                                await _teatmentTemp.SaveTreatmentTempDetails(templateDetails);
                            }
                        }
                    }

                    await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
                    await OnInitializedAsync();
                    StateHasChanged();



                }
                else
                {
                    Console.WriteLine("No data received from JavaScript.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }


        public async Task Delete(string id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                if (await _teatmentTemp.DeleteTreatmentTemp(id))
                {
                    await JS.InvokeVoidAsync("showAlert", "Delete Successful", "Record has been successfully deleted.", "success", "swal-danger");
                    await OnInitializedAsync();
                    StateHasChanged();
                }
                else
                {
                    await JS.InvokeVoidAsync("showAlert", "Opps.....", "Something Went Wrong. Please Try Again", "warning", "swal-danger");
                }

            }
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


        [JSInvokable("GetMedicines")]
        public Task<List<DrugMedicine>> LoadMedicines(string search)
        {
            var filtered = string.IsNullOrEmpty(search)
                ? Brands
                : Brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return Task.FromResult(filtered);
        }








    }
}
