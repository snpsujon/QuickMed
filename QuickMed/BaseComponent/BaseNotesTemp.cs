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
        public List<TblNotesTempDetails> templateDetails { get; set; }

        protected override async Task OnInitializedAsync()
        {
            models = await _notes.GetAsync(); // Load the initial data
            templateDetails = await _notes.GetDetailsAsync();

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
            await JS.InvokeVoidAsync("makeDataTable", "datatable-noteTemp");
        }

        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setupEditableTable", "notesMainTbl", "add_new_notes");
            await JS.InvokeVoidAsync("makeSelect2", false);
        }

        protected async Task OnSaveBtnClick()
        {
            var tableSelectedValue = await JS.InvokeAsync<List<string>>("GetTableData", "notesMainTbl");
            if (model.Id == Guid.Empty) // Check if the GUID is uninitialized
            {
                model.Id = Guid.NewGuid();              

                var isSave = await _notes.SaveAsync(model); 

                if (isSave == true)
                {
                    foreach (var item in tableSelectedValue)
                    {
                        var detailsData = new TblNotesTempDetails()
                        {
                            Id = Guid.NewGuid(),
                            TblNotesTempMasterId = model.Id,
                            Name = item
                        };
                        templateDetails.Add(detailsData);
                    }
                    var saveDetails = await _notes.SaveTemplateDetails(templateDetails);

                    await InitializeDataTable();

                    await OnInitializedAsync();

                    model = new();
                    
                    StateHasChanged();
                }

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


        protected async Task OnEditClick(TblNotesTemplate data)
        {
            model = data;
            StateHasChanged(); // Re-render the component with the updated model
        }
        protected async Task OnDeleteClick(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _notes.DeleteAsync(id);
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
