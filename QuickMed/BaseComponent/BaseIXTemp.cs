using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
    public class BaseIXTemp : ComponentBase
    {
        [Inject]
        public IIXTemp _ixTemp { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        public string saveOrUpdateContent { get; set; } = "Save";

        public TblIXTemplate model = new();
        public IEnumerable<TblIXTemplate>? models { get; set; }
        public List<TblIXDetails> templateDetails { get; set; }
        public DotNetObjectReference<BaseIXTemp> ObjectReference { get; private set; }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        await JS.InvokeVoidAsync("makeDataTable", "datatable-ixTemp");
        //        await JS.InvokeVoidAsync("setupEditableTable", "makeEditable_IxTemp", "but_add_IxTemp");
        //    }
        //}

        //protected override async Task OnInitializedAsync()
        //{
        //    //models = await _ixTemp.GetAsync(); // Load the initial data
        //}

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
            model = new();
            templateDetails = new();
            models = await _ixTemp.GetAsync();
            this.RefreshDataTable();

        }
        protected async Task InitializeJS()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            await JS.InvokeVoidAsync("setupEditableTable", "makeEditable_IxTemp", "but_add_IxTemp", false);
            await JS.InvokeVoidAsync("makeSelect2", false);
            await JS.InvokeVoidAsync("OnChangeEvent", "IXTempSelect", "IXTempChange", ObjectReference);
            await JS.InvokeVoidAsync("makeTableDragable", "makeEditable_IxTemp");
        }
        protected async Task RefreshDataTable()
        {

            var tableData = models?.Select((ix, index) => new[]
                {
                    (index + 1).ToString(), // Serial number starts from 1
                    ix.TemplateName?.ToString() ?? string.Empty, // Note name
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary dTRowActionBtn' data-id='{ix.Id}' data-method='OnEditClick'></i>
                        <i class='dripicons-trash btn btn-soft-danger dTRowActionBtn' data-id='{ix.Id}' data-method='OnDeleteClick'></i>
                    </div>
                    " // Action buttons
                }).ToArray();

            await JS.InvokeVoidAsync("makeDataTableQ", "datatable-IxTemp", tableData);

        }

        protected async Task OnSaveBtnClick()
        {
            try
            {
                var result = await JS.InvokeAsync<JsonElement>("GetIXTempData");
                if (result.ValueKind != JsonValueKind.Undefined && result.ValueKind != JsonValueKind.Null)
                {
                    if (result.TryGetProperty("templateName", out JsonElement templateNameElement))
                    {
                        var templateName = templateNameElement.GetString();
                        if (model.Id == Guid.Empty)
                        {
                            model = new()
                            {
                                Id = Guid.NewGuid(),
                                TemplateName = templateName
                            };
                            await _ixTemp.SaveAsync(model);
                        }
                        else
                        {
                            model.TemplateName = templateName;
                            await _ixTemp.UpdateAsync(model);
                            await _ixTemp.DeleteDetailsAsync(model.Id);
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

                                templateDetails = new List<TblIXDetails>();
                                foreach (var item in ListData)
                                {
                                    templateDetails.Add(new TblIXDetails
                                    {
                                        Id = Guid.NewGuid(),
                                        TblIXTempMasterId = model.Id,
                                        Name = item
                                    });
                                }
                                var saveDetails = await _ixTemp.SaveTemplateDetails(templateDetails);
                                await JS.InvokeVoidAsync("showAlert", "Save Successful", "Record has been successfully Saved.", "success", "swal-success");
                                model = new TblIXTemplate();
                                await JS.InvokeVoidAsync("ClearTable", "makeEditable_IxTemp");

                                await InitializeDataTable();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("templateName not found.");
                    }

                    StateHasChanged();


                }
            }
            catch (Exception ex)
            {

                throw;
            }




        }
        [JSInvokable("OnEditClick")]
        public async Task OnEditClick(string Id)
        {
            await OnEditClicks(models.FirstOrDefault(x => x.Id == Guid.Parse(Id)));
            StateHasChanged(); // Re-render the component with the updated model
        }
        protected async Task OnEditClicks(TblIXTemplate data)
        {
            model = data;
            await IXTempChange(data.Id.ToString());
            saveOrUpdateContent = "Update";
            StateHasChanged(); // Re-render the component with the updated model
        }
        [JSInvokable]
        public async Task IXTempChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    templateDetails = await _ixTemp.GetDataById(selectedData);
                    await JS.InvokeVoidAsync("populateIXTable", templateDetails, "makeEditable_IxTemp");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [JSInvokable("OnDeleteClick")]
        public async Task OnDeleteClick(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {
                var isDeleted = await _ixTemp.DeleteAsync(id);
                if (isDeleted)
                {
                    // Show success alert with red color
                    await JS.InvokeVoidAsync("showAlert", "Delete Successful", "Record has been successfully deleted.", "success", "swal-danger");

                    // Refresh the list after deletion
                    await InitializeDataTable();
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
