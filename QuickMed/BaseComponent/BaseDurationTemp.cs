using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseDurationTemp : ComponentBase
    {
        [Inject]
        public IDurationTemp _duration { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }


        public TblDuration duration = new();
        public IEnumerable<TblDuration>? durations { get; set; }

        protected override async Task OnInitializedAsync()
        {
            durations = await App.Database.GetTableRowsAsync<TblDuration>("TblDuration");
            durations = await _duration.GetAsync(); // Load the initial data
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
        //    await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        //}

        protected async Task RefreshDataTable()
        {

            var tableData = durations?.Select((drt, index) => new[]
                {
                    (index + 1).ToString(), // Serial number starts from 1
                    drt.Name?.ToString() ?? string.Empty, // Note name
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary' onclick='editRow({drt.Id})'></i>
                        <i class='dripicons-trash btn btn-soft-danger' onclick='deleteRow({drt.Id})'></i>
                    </div>
                    " // Action buttons
                }).ToArray();

            await JS.InvokeVoidAsync("makeDataTableQ", "datatable-durationTemp", tableData);

        }

        protected async Task OnSaveBtnClick()
        {
            if (duration.Id == Guid.Empty) // Check if the GUID is uninitialized
            {
                duration.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
                await _duration.SaveAsync(duration); // Create the new template
                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            }
            else
            {
                await _duration.UpdateAsync(duration); // Update the existing template
                await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");
            }
            duration = new TblDuration();
            await OnInitializedAsync();
            await RefreshDataTable();

            StateHasChanged();  // Update the UI
        }

        protected async Task OnEditClick(TblDuration data)
        {
            duration = data;
            StateHasChanged(); // Re-render the component with the updated model
        }
        protected async Task OnDeleteClick(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _duration.DeleteAsync(id);
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
