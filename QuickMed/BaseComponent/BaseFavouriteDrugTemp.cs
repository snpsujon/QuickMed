﻿using Microsoft.AspNetCore.Components;
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
        public async Task<List<DrugMedicine>> LoadMedicines(string search)
        {
            Brands = await _mix.GetAllMedicine(search);
            var filtered = string.IsNullOrEmpty(search)
                ? Brands
                : Brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return await Task.FromResult(filtered);
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

            try
            {
                var result = await JS.InvokeAsync<JsonElement>("GetDrugTempData");

                await SaveData(result);
                await JS.InvokeVoidAsync("showAlert", "Success", "Drug template saved successfully", "success", "swal-success");
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
                var isValid = true;
                var TemplateId = Guid.NewGuid();
                string templateName = "";
                if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
                {
                    if (result.TryGetProperty("templateName", out JsonElement templateNameElement))
                    {
                        // Extract properties and parse values
                        templateName = templateNameElement.GetString();
                        var brandSelect = result.TryGetProperty("brandSelect", out JsonElement brandSelectElement)
                                          ? brandSelectElement.GetString()
                                          : null;
                        var doseSelect = result.TryGetProperty("doseSelect", out JsonElement doseSelectElement)
                                         ? doseSelectElement.GetString()
                                         : null;
                        var instructionSelect = result.TryGetProperty("instructionSelect", out JsonElement instructionSelectElement)
                                                ? instructionSelectElement.GetString()
                                                : null;
                        var durationSelectfav = result.TryGetProperty("durationSelectfav", out JsonElement durationSelectfavElement)
                                                ? durationSelectfavElement.GetString()
                                                : null;

                        // Validate all required fields
                        if (string.IsNullOrWhiteSpace(templateName) ||
                            string.IsNullOrWhiteSpace(brandSelect) ||
                            string.IsNullOrWhiteSpace(doseSelect) ||
                            string.IsNullOrWhiteSpace(instructionSelect) ||
                            string.IsNullOrWhiteSpace(durationSelectfav) ||
                            !Guid.TryParse(brandSelect, out var brandId) ||
                            !Guid.TryParse(doseSelect, out var doseId) ||
                            !Guid.TryParse(instructionSelect, out var instructionId) ||
                            !Guid.TryParse(durationSelectfav, out var durationId))
                        {
                            // Show alert for missing or invalid input

                            await JS.InvokeVoidAsync("showAlert", "Save Failed", "Some Input fields are missing or invalid", "error", "swal-error");
                            return;
                        }

                        // Create a new drug template object
                        var drugTemp = new TblFavouriteDrugTemplate
                        {
                            Id = TemplateId,
                            Name = templateName,
                            BrandId = brandId,
                            DoseId = doseId,
                            InstructionId = instructionId,
                            DurationId = durationId,
                            CreatedAt = DateTime.Now
                        };

                        // Save the drug template
                        var applicationDbContext = new ApplicationDbContext();
                        var drugService = new FavouriteDrugService(applicationDbContext);
                        await drugService.SaveFavouriteDrugTemp(drugTemp);

                        // Show success message
                        //await JS.InvokeVoidAsync("ClearFormData");
                        //await JS.InvokeVoidAsync("showAlert", "Success", "Drug template saved successfully", "success", "swal-success");
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


        [JSInvokable("OnDeleteClick")]
        public async Task OnDeleteClick(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _drug.DeleteAsync(id);
                if (isDeleted)
                {
                    // Show success alert with red color
                    await JS.InvokeVoidAsync("showAlert", "Delete Successful", "Record has been successfully deleted.", "success", "swal-danger");

                    // Refresh the list after deletion
                    await OnInitializedAsync();
                    StateHasChanged();  // Update the UI after deletion
                }
                else
                {
                    // Handle failure case and show an error alert if necessary
                    Console.WriteLine("Failed to delete record from the database.");
                }
            }
        }

    }
}
