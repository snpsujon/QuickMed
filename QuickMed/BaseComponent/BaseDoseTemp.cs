using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseDoseTemp : ComponentBase
    {
        [Inject]
        public IDoseTemp _dose { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        public TblDose model = new();
        public IEnumerable<TblDose>? models { get; set; }

        protected override async Task OnInitializedAsync()
        {
            models = await _dose.GetAsync(); // Load the initial data
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
                await InitializeJS();
            }
        }

        protected async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        }

        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setupEditableTable", "TretmentTmpAdviceTbl", "add_Advice");
            await JS.InvokeVoidAsync("setupEditableTable", "datatable-patientList");
        }

        protected async Task OnSaveBtnClick()
        {
            if (model.Id == Guid.Empty) // Check if the GUID is uninitialized
            {
                model.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
                await _dose.SaveAsync(model); // Create the new template
                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            }
            else
            {
                await _dose.UpdateAsync(model); // Update the existing template
                await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");
            }

            // Reset the model for future input
            model = new TblDose(); // Creates a new instance with a new GUID

            // Fetch updated data
            //models = await _dXTemp.GetCCTempData();
            await OnInitializedAsync();

            StateHasChanged();  // Update the UI
        }
    }
}
