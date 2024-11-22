using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseRefferenceTemp : ComponentBase
    {
        [Inject]
        public IRefferenceTemp _ref { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }


        public TblRefferTemplate tblReffer = new();
        public IEnumerable<TblRefferTemplate>? tblReffers { get; set; }


        public DotNetObjectReference<BaseRefferenceTemp> ObjectReference { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);

            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            tblReffers = await _ref.GetAsync(); // Load the initial data
            await RefreshDataTable();


        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Initialize JavaScript-based DataTable once the component has rendered
                await InitializeJS();
            }
        }

        //protected async Task InitializeDataTable()
        //{
        //    await JS.InvokeVoidAsync("makeDataTable", "datatable-noteTemp");
        //}

        protected async Task RefreshDataTable()
        {

            var tableData = tblReffers?.Select((note, index) => new[]
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

            await JS.InvokeVoidAsync("makeDataTableQ", "datatable-reffTemp", tableData);

        }

        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("makeSelect2", false);
            await JS.InvokeVoidAsync("initializeQuill", "#editors");
            StateHasChanged();
        }


        protected async Task OnSaveBtnClick()
        {
            try
            {
                var content = await JS.InvokeAsync<string>("getQuillContent", "#editors");
                tblReffer.Details = content;
                if (tblReffer.Id != Guid.Empty)
                {
                    await App.Database.UpdateAsync(tblReffer);
                }
                else
                {
                    tblReffer.Id = Guid.NewGuid();
                    await App.Database.CreateAsync(tblReffer);
                }
                tblReffer = new();
                // Clear the Quill editor after saving
                await JS.InvokeVoidAsync("clearQuillContent", "#editors");
                await OnInitializedAsync();

                StateHasChanged();

            }
            catch (Exception)
            {

                throw;
            }
        }

        [JSInvokable("OnEditClick")]
        public async Task OnEditClick(string Id)
        {
            // Fetch the data based on the ID
            var data = tblReffers.FirstOrDefault(x => x.Id == Guid.Parse(Id));
            if (data != null)
            {
                tblReffer.Name = data.Name;
                tblReffer.Id = Guid.Parse(Id);
                tblReffer.Details = data.Details;

                // Pass the details content to the Quill editor
                await JS.InvokeVoidAsync("setQuillContent", "#editors", tblReffer.Details);
            }

            // Re-render the component with the updated model
            StateHasChanged();
        }

        [JSInvokable("OnDeleteClick")]
        public async Task OnDeleteClick(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _ref.DeleteAsync(id);
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
