using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.BaseComponent
{
    public class BasePrescriptionList : ComponentBase
    {
        [Inject]
        public IPrescription _pres { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }

        public PrescriptionVM prescription = new();
        public IEnumerable<PrescriptionVM>? prescriptions { get; set; }
        protected override async Task OnInitializedAsync()
        {
            prescriptions = await _pres.GetAll();
            await RefreshDataTable();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
            }
        }

        //protected async Task InitializeDataTable()
        //{
        //    await JS.InvokeVoidAsync("makeDataTable", "datatable-prescriptionList");

        //}

        protected async Task RefreshDataTable()
        {

            var tableData = prescriptions?.Select((pres, index) => new[]
                {
                    (index + 1).ToString(), // Serial number starts from 1
                    pres.PrescriptionCode?.ToString() ?? string.Empty, // Note name
                    pres.PrescriptionDate.HasValue
                    ? pres.PrescriptionDate.Value.ToString("yyyy-MM-dd") // Format the DateTime
                    : string.Empty, // Handle null DateTime
                    pres.PatientName?.ToString() ?? string.Empty,
                    pres.MobileNumber?.ToString() ?? string.Empty,
                    pres.Address?.ToString() ?? string.Empty,
                    pres.Dx?.ToString() ?? string.Empty,
                    pres.Plan?.ToString() ?? string.Empty,
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary' onclick='editRow({pres.Id})'></i>
                        <i class='dripicons-trash btn btn-soft-danger' onclick='deleteRow({pres.Id})'></i>
                    </div>
                    " // Action buttons
                }).ToArray();

            if (tableData != null)
            {
                await JS.InvokeVoidAsync("makeDataTableQ", "datatable-prescriptionList", tableData);
            }

        }
        protected async Task OnPreviewClick(PrescriptionVM data)
        {
            //model = data;
            //await NotesTempChange(data.Id.ToString());
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
