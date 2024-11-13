using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseDXTemp : ComponentBase
    {
        [Inject]
        public IDXTemp _dXTemp { get; set; }

        public TblDXTemplate model = new();
        public IEnumerable<TblDXTemplate>? models { get; set; }


        [Inject]
        public IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeDataTable();
            models = await _dXTemp.GetCCTempData();

        }

        private async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-dxTemp");
        }



        protected async Task OnSaveBtnClick()
        {
            if (model.Id == Guid.Empty) // Check if the GUID is uninitialized
            {
                model.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
                await _dXTemp.SaveCCTemplate(model); // Create the new template
                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            }
            else
            {
                await _dXTemp.UpdateCCTemplate(model); // Update the existing template
                await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");
            }

            // Reset the model for future input
            model = new TblDXTemplate(); // Creates a new instance with a new GUID

            // Fetch updated data
            models = await _dXTemp.GetCCTempData();
            await InitializeDataTable();  // Re-initialize DataTable after data change
            StateHasChanged();  // Update the UI
        }
        protected async Task OnEditClick(TblDXTemplate data)
        {
            model = data;
            StateHasChanged(); // Re-render the component with the updated model
        }
        protected async Task OnDeleteClick(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _dXTemp.DeleteAsync(id);
                if (isDeleted)
                {
                    // Show success alert with red color
                    await JS.InvokeVoidAsync("showAlert", "Delete Successful", "Record has been successfully deleted.", "success", "swal-danger");

                    // Refresh the list after deletion
                    models = await _dXTemp.GetCCTempData();
                    await InitializeDataTable();  // Re-initialize DataTable after deletion
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
