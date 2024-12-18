﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseCCTemp : ComponentBase
    {
        [Inject]
        public ICCTemp _cCTemp { get; set; }

        public TblCCTemplate tblCCTemplate = new();
        public IEnumerable<TblCCTemplate>? tblCCTemplates { get; set; }


        [Inject]
        public IJSRuntime JS { get; set; }
        public DotNetObjectReference<BaseCCTemp> ObjectReference { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            tblCCTemplates = await _cCTemp.GetCCTempData();
            await RefreshDataTable();

        }

        //private async Task InitializeDataTable()
        //{
        //    await JS.InvokeVoidAsync("makeDataTable", "datatable-ccTemp");
        //}
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
            }
        }
        protected async Task RefreshDataTable()
        {

            var tableData = tblCCTemplates?.Select((note, index) => new[]
                {
                    (index + 1).ToString(), // Serial number starts from 1
                    note.Name?.ToString() ?? string.Empty, // Note name
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary dTRowActionBtn' data-id='{note.Id}' data-method='OnEditClick'></i>
                        <i class='dripicons-trash btn btn-soft-danger dTRowActionBtn' data-id='{note.Id}' data-method='OnDeleteClick'></i>
                    </div>
                    " // Action buttons
                }).ToArray();

            await JS.InvokeVoidAsync("makeDataTableQ", "datatable-ccTemp1", tableData);

        }


        protected async Task OnSaveBtnClick()
        {
            if (tblCCTemplate.Id == Guid.Empty) // Check if the GUID is uninitialized
            {
                tblCCTemplate.Id = Guid.NewGuid(); // This line will be redundant as the default is already set
                await _cCTemp.SaveCCTemplate(tblCCTemplate); // Create the new template
                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
            }
            else
            {
                await _cCTemp.UpdateCCTemplate(tblCCTemplate); // Update the existing template
                await JS.InvokeVoidAsync("showAlert", "Update Successful", "Record has been successfully Updated.", "success", "swal-info");
            }

            // Reset the model for future input
            tblCCTemplate = new TblCCTemplate(); // Creates a new instance with a new GUID

            // Fetch updated data
            tblCCTemplates = await _cCTemp.GetCCTempData();
            await RefreshDataTable();   // Re-initialize DataTable after data change
            StateHasChanged();  // Update the UI
        }



        [JSInvokable("OnEditClick")]
        public async Task OnEditClick(string Id)
        {

            tblCCTemplate.Name = tblCCTemplates.FirstOrDefault(x => x.Id == Guid.Parse(Id)).Name;
            tblCCTemplate.Id = Guid.Parse(Id);
            StateHasChanged(); // Re-render the component with the updated model
        }
        [JSInvokable("OnDeleteClick")]
        public async Task OnDeleteClick(Guid id)
        {


            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _cCTemp.DeleteAsync(id);
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
