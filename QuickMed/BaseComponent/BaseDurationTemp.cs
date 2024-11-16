﻿using Microsoft.AspNetCore.Components;
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


        public TblDuration model = new();
        public IEnumerable<TblDuration>? models { get; set; }

        protected override async Task OnInitializedAsync()
        {
            models = await _duration.GetAsync(); // Load the initial data
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
            await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        }

        protected async Task OnSaveBtnClick()
        {
            if (model.Id == Guid.Empty) // Check if the GUID is uninitialized
            {
                model.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
                await _duration.SaveAsync(model); // Create the new template
                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            }
            else
            {
                await _duration.UpdateAsync(model); // Update the existing template
                await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");
            }
            model = new TblDuration();
            await OnInitializedAsync();

            StateHasChanged();  // Update the UI
        }

        protected async Task OnEditClick(TblDuration data)
        {
            model = data;
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