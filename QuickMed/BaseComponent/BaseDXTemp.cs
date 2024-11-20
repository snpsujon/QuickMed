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


        public TblDXTemplate dxtemp = new();
        public IEnumerable<TblDXTemplate>? dxtemps { get; set; }


        [Inject]
        public IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            dxtemps = await App.Database.GetTableRowsAsync<TblDXTemplate>("TblDXTemplate");
            dxtemps = await _dXTemp.GetCCTempData();

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
            }
        }

        //private async Task InitializeDataTable()
        //{
        //    await JS.InvokeVoidAsync("makeDataTable", "datatable-dxTemp");
        //}
        protected async Task RefreshDataTable()
        {

            var tableData = dxtemps?.Select((dx, index) => new[]
                {
                    (index + 1).ToString(), // Serial number starts from 1
                    dx.Name?.ToString() ?? string.Empty, // Note name
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary' onclick='editRow({dx.Id})'></i>
                        <i class='dripicons-trash btn btn-soft-danger' onclick='deleteRow({dx.Id})'></i>
                    </div>
                    " // Action buttons
                }).ToArray();

            await JS.InvokeVoidAsync("makeDataTableQ", "datatable-dxTemp", tableData);

        }

        private async Task DownloadFile(string fileName, byte[] fileContent)
        {
            var base64File = Convert.ToBase64String(fileContent);
            await JS.InvokeVoidAsync("downloadFileFromBytes", fileName, base64File);
        }


        protected async Task OnSaveBtnClick()
        {
            if (dxtemp.Id == Guid.Empty) // Check if the GUID is uninitialized
            {
                dxtemp.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
                await _dXTemp.SaveCCTemplate(dxtemp); // Create the new template
                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            }
            else
            {
                await _dXTemp.UpdateCCTemplate(dxtemp); // Update the existing template
                await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");
            }

            // Reset the model for future input
            dxtemp = new TblDXTemplate(); // Creates a new instance with a new GUID

            await OnInitializedAsync();
            await RefreshDataTable();
            StateHasChanged();  // Update the UI
        }
        protected async Task OnEditClick(TblDXTemplate data)
        {
            dxtemp = data;
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
