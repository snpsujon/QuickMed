using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.Services;
using QuickMed.ViewModels;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
    public class BaseFavouriteDrugTemp : ComponentBase
    {
        [Inject]
        public IMixTemp _mix { get; set; }

        [Inject]
        public ITeatmentTemp _teatmentTemp { get; set; }


        [Inject]
        public IFavouriteDrug _drug { get; set; }

        public DotNetObjectReference<BaseFavouriteDrugTemp> ObjectReference { get; private set; }
        public TblFavouriteDrugTemplate drugTemp = new();
        public IEnumerable<TblFavouriteDrugTemplate>? drugTemps { get; set; }
        public IEnumerable<FavouriteDrugTempVM>? drugVM { get; set; }
        public List<DrugMedicine> Brands = new List<DrugMedicine>();
        public List<TblDose> Dose = new List<TblDose>();
        public List<TblDuration> Duration = new List<TblDuration>();
        public List<TblInstruction> Instructions = new List<TblInstruction>();


        [Inject]
        public IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);

            drugTemps = await App.Database.GetTableRowsAsync<TblFavouriteDrugTemplate>("TblFavouriteDrugTemplate");
            drugVM = await _drug.GetAsync();
            Brands = new();
            Brands = await _mix.GetAllMedicine();
            Dose = new();
            Dose = await App.Database.GetTableRowsAsync<TblDose>("TblDose");
            Duration = new();
            Duration = await App.Database.GetTableRowsAsync<TblDuration>("TblDuration");
            Instructions = new();
            Instructions = await App.Database.GetTableRowsAsync<TblInstruction>("TblInstruction");
            await RefreshDataTable();

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                await InitializeJS();
            }
        }

        //private async Task InitializeDataTable()
        //{
        //    await JS.InvokeVoidAsync("makeDataTable", "datatable-dxTemp");
        //}

        protected async Task RefreshDataTable()
        {

            var tableData = drugVM?.Select((drug, index) => new[]
                {
                    (index + 1).ToString(), // Serial number starts from 1
                    drug.Name?.ToString() ?? string.Empty, // Note name
                    drug.BrandName?.ToString() ?? string.Empty,
                    drug.DoseName?.ToString() ?? string.Empty,
                    drug.InstructionName?.ToString() ?? string.Empty,
                    drug.DurationName?.ToString() ?? string.Empty,
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary dTRowActionBtn' data-id='{drug.Id}' data-method='OnEditClick'></i>
                        <i class='dripicons-trash btn btn-soft-danger dTRowActionBtn' data-id='{drug.Id}' data-method='OnDeleteClick'></i>
                    </div>
                    " // Action buttons
                }).ToArray();

            if (tableData != null)
            {
                await JS.InvokeVoidAsync("makeDataTableQ", "datatable-favouriteTemp", tableData);
            }

        }

        [JSInvokable("OnEditClick")]
        public async Task OnEditClick(string Id)
        {
            try
            {
                await JS.InvokeVoidAsync("toggleButtonVisibility", true);
                var masterData = await _drug.GetDataById(Id);
                await JS.InvokeVoidAsync("setFavMasterData", (object)masterData);


            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task CancelTemplate()
        {
            await JS.InvokeVoidAsync("toggleButtonVisibility", false);
            await JS.InvokeVoidAsync("ClearAllFields");
        }
        public async Task UpdateTemplate()
        {
            try
            {
                var result = await JS.InvokeAsync<JsonElement>("GetDrugTempData");
                if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
                {
                    var tempId = result.GetProperty("tempId").GetString();
                    if (tempId != "NewCreated")
                    {
                        string templateName = "";
                        if (result.TryGetProperty("templateName", out JsonElement templateNameElement))
                        {
                            templateName = templateNameElement.GetString();
                            var brandId = Guid.Parse(result.GetProperty("brandSelect").GetString());
                            var doseId = Guid.Parse(result.GetProperty("doseSelect").GetString());
                            var instructionId = Guid.Parse(result.GetProperty("instructionSelect").GetString());
                            var durationId = Guid.Parse(result.GetProperty("durationSelectfav").GetString());
                            drugTemp = new();
                            drugTemp = new TblFavouriteDrugTemplate
                            {
                                Id = Guid.Parse(tempId),
                                Name = templateName,
                                BrandId = brandId,
                                DoseId = doseId,
                                InstructionId = instructionId,
                                DurationId = durationId
                            };
                            await App.Database.UpdateAsync<TblFavouriteDrugTemplate>(drugTemp);
                        }
                        else
                        {
                            Console.WriteLine("templateName not found.");
                        }
                        await JS.InvokeVoidAsync("ClearAllFields");
                        await JS.InvokeVoidAsync("toggleButtonVisibility", false);
                        await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");

                        await OnInitializedAsync();
                        StateHasChanged();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            await JS.InvokeVoidAsync("makeSelect2", false);
            await JS.InvokeVoidAsync("makeSelect2Custom", "select2C", "GetMedicines", 3);
        }

        //[JSInvokable("GetMedicines")]
        //public Task<List<DrugMedicine>> LoadMedicines(string search)
        //{
        //    var filtered = string.IsNullOrEmpty(search)
        //        ? Brands
        //        : Brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

        //    return Task.FromResult(filtered);
        //}

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
                    await JS.InvokeVoidAsync("populateTreatmentTable", treatments, "TretmentTmpTbl");

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        protected async Task OnSaveBtnClick()
        {
            //var result = await JS.InvokeAsync<JsonElement>("GetFavDrugTempData");

            //if (drugTemp.Id == Guid.Empty) // Check if the GUID is uninitialized
            //{
            //    drugTemp.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
            //    await _drug.SaveFavouriteDrugTemp(drugTemp); // Create the new template
            //    await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            //}
            //drugTemp = new TblFavouriteDrugTemplate(); // Creates a new instance with a new GUID
            //await OnInitializedAsync();
            //StateHasChanged();  // Update the UI

            try
            {
                var result = await JS.InvokeAsync<JsonElement>("GetDrugTempData");
                await SaveData(result);
                await OnInitializedAsync();
                await RefreshDataTable();
                StateHasChanged();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SaveData(JsonElement result)
        {
            try
            {
                var TemplateId = Guid.NewGuid();
                string templateName = "";
                if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
                {
                    if (result.TryGetProperty("templateName", out JsonElement templateNameElement))
                    {
                        templateName = templateNameElement.GetString();
                        var brandId = Guid.Parse(result.GetProperty("brandSelect").GetString());
                        var doseId = Guid.Parse(result.GetProperty("doseSelect").GetString());
                        var instructionId = Guid.Parse(result.GetProperty("instructionSelect").GetString());
                        var durationId = Guid.Parse(result.GetProperty("durationSelectfav").GetString());
                        drugTemp = new();
                        drugTemp = new TblFavouriteDrugTemplate
                        {
                            Id = TemplateId,
                            Name = templateName,
                            BrandId = brandId,
                            DoseId = doseId,
                            InstructionId = instructionId,
                            DurationId = durationId
                        };
                        //[Inject]
                        //public IFavouriteDrug _drug { get; set; }
                        ApplicationDbContext applicationDbContext = new ApplicationDbContext();
                        IFavouriteDrug drugService = new FavouriteDrugService(applicationDbContext);
                        await drugService.SaveFavouriteDrugTemp(drugTemp);
                    }
                    else
                    {
                        Console.WriteLine("templateName not found.");
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [JSInvokable("SaveFavOusud")]
        public async Task<bool> SaveFavOusud(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<JsonElement>(data);
                await SaveData(result);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }



    }
}
