using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
    public class BaseAppoinment : ComponentBase
    {
        [Inject]
        public IAppoinment _appoinment { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }



        public PatientVM model = new();
        public IEnumerable<PatientVM>? models { get; set; }
        public DotNetObjectReference<BaseAppoinment> ObjectReference { get; private set; }



        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            models = await _appoinment.GetAsync(); // Load the initial data
            await RefreshDataTable();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        await RefreshDataTable();
        //    }
        //}

        //protected async Task InitializeDataTable()
        //{
        //    await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        //}

        protected async Task RefreshDataTable()
        {

            var tableData = models?.Select((appoin, index) => new[]
                {
                    (index + 1).ToString(), // Serial number starts from 1
                    appoin.Name?.ToString() ?? string.Empty, // Note name
                    appoin.Age?.ToString() ?? string.Empty, // Note name
                    appoin.Gender?.ToString() ?? string.Empty, // Note name
                    appoin.Address?.ToString() ?? string.Empty, // Note name
                    appoin.Mobile?.ToString() ?? string.Empty, // Note name
                    appoin.Code?.ToString() ?? string.Empty, // Note name
                    appoin.Weight?.ToString() ?? string.Empty, // Note name
                    appoin.AdmissionDate.ToString("yyyy-MM-dd"),

                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary dTRowActionBtn' data-id='{appoin.Id}' data-method='OnEditClick'></i>
                        <i class='dripicons-trash btn btn-soft-danger dTRowActionBtn' data-id='{appoin.Id}' data-method='OnDeleteClick'></i>
                    </div>
                    " // Action buttons
                }).ToArray();

            await JS.InvokeVoidAsync("makeDataTableQ", "table-patientList", tableData);

        }



        protected async Task OnSaveBtnClick()
        {

            model.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
            await _appoinment.SaveAsync(model); // Create the new template
            await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");

            //if (model.Id == Guid.Empty) // Check if the GUID is uninitialized
            //{
            //    model.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
            //    await _appoinment.SaveAsync(model); // Create the new template
            //    await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            //}
            //else
            //{
            //    await _appoinment.UpdateAsync(model); // Update the existing template
            //    await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");
            //}

            // Reset the model for future input
            model = new PatientVM(); // Creates a new instance with a new GUID

            // Fetch updated data
            //models = await _dXTemp.GetCCTempData();
            await OnInitializedAsync();

            StateHasChanged();  // Update the UI
        }

        [JSInvokable("OnEditClick")]
        public async Task OnEditClick(dynamic data)
        {
            if (data.ValueKind == JsonValueKind.String)
            {
                var actualValue = data.GetString();
                Guid actualGuid = Guid.Empty;
                if (!string.IsNullOrEmpty(actualValue) && Guid.TryParse(actualValue, out actualGuid))
                {
                    var checkExist = models.FirstOrDefault(x => x.Id == actualGuid);

                    if (checkExist != null)
                    {
                        checkExist.Gender = checkExist.Gender == "M" ? "1" : checkExist.Gender == "F" ? "2" : "3";
                        model = checkExist;

                        // Notify the framework of state changes to update the UI
                        StateHasChanged();
                    }
                    else
                    {
                        Console.WriteLine("No matching model found for the provided GUID.");
                    }
                }
                else
                {
                    Console.WriteLine("The provided value is either null, empty, or not a valid GUID.");
                }
            }
            else
            {
                Console.WriteLine("The data is not a string. Unable to parse as GUID.");
            }




        }

        [JSInvokable("OnDeleteClick")]
        public async Task OnDeleteClick(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _appoinment.DeleteAsync(id);
                if (isDeleted)
                {
                    // Show success alert with red color
                    await JS.InvokeVoidAsync("showAlert", "Delete Successful", "Record has been successfully deleted.", "success", "swal-danger");

                    // Refresh the list after deletion
                    await OnInitializedAsync();
                    //model = new PatientVM();
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
