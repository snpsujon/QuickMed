using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
    public class BaseMixTemp : ComponentBase
    {
        [Inject]
        public IMixTemp _mix { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public ITeatmentTemp _teatmentTemp { get; set; }
        public DotNetObjectReference<BaseMixTemp> ObjectReference { get; private set; }



        public TblMixTemplate mixTemplate = new();
        public DrugMedicine brnad = new();
        public TblDose dose = new();
        public List<TblDuration>? Duration { get; set; }
        public IEnumerable<TblMixTemplate>? mixTemplates { get; set; }
        public List<TblMixTempDetails> mixtemplateDetails { get; set; }
        public List<DrugMedicine>? brands { get; set; }
        public List<TblDose>? doses { get; set; }
        public List<TblInstruction>? Instructions { get; set; }
        public List<TblInstruction>? templateDetails { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            mixTemplates = await App.Database.GetTableRowsAsync<TblMixTemplate>("TblMixTemplate");
            brands = await _mix.GetAllMedicine();
            doses = await App.Database.GetTableRowsAsync<TblDose>("TblDose");

            Duration = await App.Database.GetTableRowsAsync<TblDuration>("TblDuration");

            Instructions = await App.Database.GetTableRowsAsync<TblInstruction>("TblInstruction");




        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshDataTable();
                //await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
                await InitializeJS();
            }
        }

        protected async Task RefreshDataTable()
        {

            var tableData = mixTemplates?.Select(mt => new[]
            {
                mt.Name?.ToString() ?? string.Empty, // Replace Property1 with actual property name
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary' onclick='editRow({mt.Id})'></i>
                        <i class='dripicons-trash btn btn-soft-danger' onclick='deleteRow({mt.Id})'></i>
                    </div>
                    "  // Replace Property2 with actual property name
            }).ToArray();





            await JS.InvokeVoidAsync("makeDataTableQ", "datatable-mixTemp", tableData);

        }

        protected async Task InitializeJS()
        {


            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            await JS.InvokeVoidAsync("setupEditableTable", "MixTempTbl", "add_MixTemp");
            await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");
            await JS.InvokeVoidAsync("makeSelect2", true);
            await JS.InvokeVoidAsync("makeSelect2Custom", "select2C", "GetMedicines", 3);

        }

        public async Task AddNewPlusBtn()
        {
            try
            {
                var result = await JS.InvokeAsync<object>("AddNewPlusBtn");
                if (result is not null)
                {
                    var jsonString = result.ToString();


                    var treatments = JsonSerializer.Deserialize<List<TreatmentPopVM>>(jsonString);

                    await JS.InvokeVoidAsync("populateMixTempTable", treatments, "MixTempTbl");

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [JSInvokable("GetMedicines")]
        public Task<List<DrugMedicine>> LoadMedicines(string search)
        {
            var filtered = string.IsNullOrEmpty(search)
                ? brands
                : brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return Task.FromResult(filtered);
        }
        [JSInvokable("GetOusudData")]
        public async Task<dynamic> GetOusudData(string ousudData)
        {
            var sameGen = await _teatmentTemp.GetBrandsSameGenaric(ousudData);

            var result = new
            {
                Ousud = sameGen,
                Dose = doses.Select(x => new SelectVM
                {
                    text = x.Name,
                    value = x.Id
                }).ToList(),
            };
            return result;

            // Example data
            //return new List<string> { "Medicine 1", "Medicine 2", "Medicine 3" };
        }




        public async Task MixTempSave()
        {
            try
            {
                var result = await JS.InvokeAsync<JsonElement>("GetMixTempData");

                var TemplateId = Guid.NewGuid();
                string templateName = "";

                if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
                {

                    if (result.TryGetProperty("templateName", out JsonElement templateNameElement))
                    {
                        templateName = templateNameElement.GetString();
                        var nn = result.GetProperty("doseInsSelect").GetString();
                        mixTemplate = new();
                        mixTemplate = new TblMixTemplate
                        {
                            Id = TemplateId,
                            Name = templateName,
                            Dose = result.GetProperty("doseInsSelect").GetString(),
                            Duration = result.GetProperty("durationInsSelect").GetString(),
                            TotalQty = result.GetProperty("totalqty").GetString(),
                            Instruction = result.GetProperty("instructionInsSelect").GetString(),
                            Notes = result.GetProperty("notesIns").GetString()


                        };
                        await _mix.SaveMixTemp(mixTemplate);
                    }


                    if (result.TryGetProperty("tempData", out JsonElement jsonelement))
                    {
                        if (jsonelement.ValueKind == JsonValueKind.Array)
                        {
                            var ListData = jsonelement.EnumerateArray()
                                .Select(item => new
                                {
                                    Index = item.GetProperty("index").GetInt32(),
                                    Brand = item.GetProperty("brand").GetProperty("value").GetString(),
                                    Dose = item.GetProperty("dose").GetProperty("value").GetString()
                                }).ToList();

                            if (ListData.Count() > 0)
                            {

                                mixtemplateDetails = new List<TblMixTempDetails>();
                                foreach (var item in ListData)
                                {
                                    mixtemplateDetails.Add(new TblMixTempDetails
                                    {
                                        Id = Guid.NewGuid(),
                                        TblMixTemplateMasterId = mixTemplate.Id,
                                        BrandId = item.Brand,
                                        DoseId = item.Dose,

                                    });
                                }
                                var saveDetails = await _mix.SaveMixTempDetails(mixtemplateDetails);
                                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("templateName not found.");
                    }

                    await OnInitializedAsync();
                    await RefreshDataTable();
                    StateHasChanged();
                    await JS.InvokeVoidAsync("ClearTable", "MixTempTbl");


                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
