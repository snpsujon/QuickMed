using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.BaseComponent
{
    public class BaseNotesTemp : ComponentBase
    {
        [Inject]
        public INotesTemp _notes { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }


        public TblNotesTemplate model = new();
        public IEnumerable<TblNotesTemplate>? models { get; set; }

        protected override async Task OnInitializedAsync()
        {
            models = await _notes.GetAsync(); // Load the initial data
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
            await JS.InvokeVoidAsync("makeSelect2", false);
        }

        protected async Task OnSaveBtnClick()
        {
            if (model.Id == Guid.Empty) // Check if the GUID is uninitialized
            {
                model.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
                await _notes.SaveAsync(model); // Create the new template
                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            }
            else
            {
                await _notes.UpdateAsync(model); // Update the existing template
                await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");
            }

            // Reset the model for future input
            model = new TblNotesTemplate(); // Creates a new instance with a new GUID

            // Fetch updated data
            //models = await _dXTemp.GetCCTempData();
            await OnInitializedAsync();

            StateHasChanged();  // Update the UI
        }
    }
}
