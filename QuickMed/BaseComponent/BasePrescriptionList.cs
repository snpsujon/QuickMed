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
            prescriptions = await App.Database.GetTableRowsAsync<PrescriptionVM>("TblPrescription");
            await InitializeDataTable();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
            }
        }

        protected async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-prescriptionList");

        }



        protected async Task OnEditClick(PrescriptionVM data)
        {
            prescription = data;
            //await NotesTempChange(data.Id.ToString());
            StateHasChanged(); // Re-render the component with the updated model
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
