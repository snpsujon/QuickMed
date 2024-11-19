using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static SQLite.SQLite3;

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
            var objRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("setupEditableTable", "notesMainTbl", "add_new_notes",false);
            await JS.InvokeVoidAsync("makeSelect2", false);
            await JS.InvokeVoidAsync("OnChangeEvent", "notesTempSelect", "NotesTempChange", objRef);
            await JS.InvokeVoidAsync("makeTableDragable", "notesMainTbl");
        }
        [JSInvokable]
        public async Task NotesTempChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    templateDetails = await _notes.GetDataById(selectedData);
                    await JS.InvokeVoidAsync("populateNotesTable", templateDetails, "notesMainTbl");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected async Task OnSaveBtnClick()
        {
            var result = await JS.InvokeAsync<JsonElement>("GetNotesTempData");
            if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
            {
                if (result.TryGetProperty("templateName", out JsonElement templateNameElement))
                {
                   var templateName = templateNameElement.GetString();
                    if(model.Id == Guid.NewGuid())
                    {
                        model = new()
                        {
                            Id = Guid.NewGuid(),
                            Name = templateName
                        };
                        await _notes.SaveAsync(model);
                    }
                    else
                    {
                        model.Name = templateName;
                        await _notes.UpdateAsync(model);
                        await _notes.DeleteDetailsAsync(model.Id);
                    }
                    

                }

                if (result.TryGetProperty("tempData", out JsonElement jsonelement))
                {
                    if (jsonelement.ValueKind == JsonValueKind.Array)
                    {
                        var ListData = jsonelement.EnumerateArray()
                            .Select(item => item.GetString())
                            .ToList();

                        if (ListData.Count() > 0)
                        {                           
                           
                           templateDetails = new List<TblNotesTempDetails>();
                            foreach (var item in ListData)
                            {
                                templateDetails.Add(new TblNotesTempDetails
                                {
                                    Id = Guid.NewGuid(),
                                    TblNotesTempMasterId = model.Id,
                                    Name = item
                                });
                            }
                            var saveDetails = await _notes.SaveTemplateDetails(templateDetails);
                            await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("templateName not found.");
                }

                await InitializeDataTable();

                await OnInitializedAsync();               

                StateHasChanged();


            }
        
        
        
        }


        protected async Task OnEditClick(TblNotesTemplate data)
        {
            model = data;
            await NotesTempChange(data.Id.ToString());
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
