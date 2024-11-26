using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.BaseComponent
{
    public class BasePrescriptionList : ComponentBase
    {
        [Inject]
        public IMixTemp _mix { get; set; }
        [Inject]
        public IPrescription _pres { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }

        public DotNetObjectReference<BasePrescriptionList> ObjectReference { get; private set; }
        public PrescriptionVM prescription = new();
        public IEnumerable<PrescriptionVM>? prescriptions { get; set; }
        public List<TblDXTemplate> Dxs = new List<TblDXTemplate>();
        public List<DrugMedicine> Brands = new List<DrugMedicine>();
        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            Dxs = new();
            Dxs = await App.Database.GetTableRowsAsync<TblDXTemplate>("TblDXTemplate");
            Brands = new();
            Brands = await _mix.GetAllMedicine();
            prescriptions = await _pres.GetAll();
            await RefreshDataTable();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
                await InitializeJS();
            }
        }
        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            await JS.InvokeVoidAsync("makeSelect2", false);
            await JS.InvokeVoidAsync("makeSelect2Custom", "select2C", "GetMedicines", 3);

        }
        [JSInvokable("GetMedicines")]
        public Task<List<DrugMedicine>> LoadMedicines(string search)
        {
            var filtered = string.IsNullOrEmpty(search)
                ? Brands
                : Brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return Task.FromResult(filtered);
        }

        protected async Task OnClearClick()
        {
            try
            {
                await JS.InvokeVoidAsync("clearForm", "searchPrescriptionForm");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        protected async Task OnSearchClick()
        {
            try
            {
                await JS.InvokeVoidAsync("getSearchInput");
                //var searchValue = await JS.InvokeAsync<dynamic>("getSearchInput");

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected async Task RefreshDataTable()
        {
            // Ensure prescriptions is not null
            if (prescriptions == null || !prescriptions.Any())
            {
                // If null or empty, pass an empty array to the JavaScript function
                await JS.InvokeVoidAsync("makeDataTableQ", "datatable-prescriptionList", new string[0][]);
                return;
            }

            var tableData = prescriptions.Select((pres, index) => new[]
            {
        (index + 1).ToString(), // Serial number starts from 1
        pres?.PrescriptionCode?.ToString() ?? string.Empty, // Safely access PrescriptionCode
        pres?.PrescriptionDate.HasValue == true
            ? pres.PrescriptionDate.Value.ToString("yyyy-MM-dd") // Format the DateTime
            : string.Empty, // Handle null DateTime
        pres?.PatientName?.ToString() ?? string.Empty, // Safely access PatientName
        pres?.MobileNumber?.ToString() ?? string.Empty, // Safely access MobileNumber
        pres?.Address?.ToString() ?? string.Empty, // Safely access Address
        pres?.Dx?.ToString() ?? string.Empty, // Safely access Dx
        pres?.Plan?.ToString() ?? string.Empty, // Safely access Plan
        pres != null
            ? $@"
                <div style='display: flex; justify-content: flex-end;'>
                    <i class='dripicons-pencil btn btn-soft-primary' onclick='editRow({pres.Id})'></i>
                    <i class='dripicons-trash btn btn-soft-danger' onclick='deleteRow({pres.Id})'></i>
                </div>
              "
            : string.Empty // Handle null prescription object for actions
    }).ToArray();

            // Ensure the JavaScript function is called only if tableData is valid
            if (tableData != null)
            {
                await JS.InvokeVoidAsync("makeDataTableQ", "datatable-prescriptionList", tableData);
            }
        }

        protected async Task OnPreviewClick(PrescriptionVM data)
        {
            StateHasChanged(); // Re-render the component with the updated model
        }


        protected async Task OnDeleteClick(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _pres.DeleteAsync(id);
                if (isDeleted)
                {
                    await JS.InvokeVoidAsync("showAlert", "Delete Successful", "Record has been successfully deleted.", "success", "swal-danger");
                    await OnInitializedAsync();
                }
                else
                {
                    Console.WriteLine("Failed to delete record from the database.");
                }
            }
        }

    }
}
