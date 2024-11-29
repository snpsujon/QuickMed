using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseAdvice : ComponentBase
    {
        public DotNetObjectReference<BaseAdvice> ObjectReference { get; private set; }
        [Inject]
        public IAdvice _advice { get; set; }
        public TblAdviceTemplate adviceTemplate = new();
        public string saveOrUpdateContent { get; set; } = "Save";
        public List<TblAdviceTemplateDetails> templateDetails = new List<TblAdviceTemplateDetails>();
        public IEnumerable<TblAdviceMaster>? masterData { get; set; }
        public IEnumerable<TblAdviceTemplate> templateListData { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            masterData = await _advice.GetAdviceMasterData();
            await JS.InvokeVoidAsync("onInitTable", "mainTable-advice", masterData);
            await JS.InvokeVoidAsync("initializeButtonClick", masterData);
            templateListData = await _advice.GetAdviceTemplateData();
            //await JS.InvokeVoidAsync("makeDataTable", "adviceListtable");
            await RefreshDataTable();
        }


        protected async Task RefreshDataTable()
        {
            var clic = $@"@onclick=""() => onDataEdit(item.Id)""";

            var tableData = templateListData?.Select(mt => new[]
            {
                mt.AdviceTemplateName?.ToString() ?? string.Empty, // Replace Property1 with actual property name
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                       <i class='dripicons-pencil btn btn-soft-primary dTRowActionBtn' data-id='{mt.Id}' data-method='onDataEdit'></i>
                        <i class='dripicons-trash btn btn-soft-danger dTRowActionBtn' data-id='{mt.Id}' data-method='onDataDelete'></i>
                    </div>
                    "  // Replace Property2 with actual property name
            }).ToArray();





            await JS.InvokeVoidAsync("makeDataTableQ", "adviceListtable", tableData);

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // await JS.InvokeVoidAsync("setupEditableTable", "mainTable-advice", "but_add");
                await JS.InvokeVoidAsync("makeTableDragable", "mainTable-advice");

                //await JS.InvokeVoidAsync("setupEditableTableWithoutButton", "mainTable-advice");
                await JS.InvokeVoidAsync("makeSelect2", true);

                StateHasChanged();
            }
        }
        protected async Task onAdviceSave()
        {
            var tableSelectedValue = await JS.InvokeAsync<List<string>>("GetTableData", "mainTable-advice");

            if (adviceTemplate.Id == Guid.Empty)
            {
                adviceTemplate.Id = Guid.NewGuid();
                if (adviceTemplate.AdviceTemplateName == null)
                {
                    adviceTemplate.AdviceTemplateName = await JS.InvokeAsync<string>("GenerateAdviceTemplateName");
                }
                var isSave = await _advice.SaveAdviceTemplate(adviceTemplate);
            }
            else
            {
                var updateTemplate = await _advice.UpdateAdviceTemplate(adviceTemplate);
                if (updateTemplate == true)
                {
                    var SqlDetails = $"DELETE FROM TblAdviceTemplateDetails WHERE AdviceTemplateId = '{adviceTemplate.Id}'";
                    var isDeleteDetails = await _advice.DeleteAdviceDetails(SqlDetails);
                }

            }
            templateDetails = new List<TblAdviceTemplateDetails>();
            foreach (var item in tableSelectedValue)
            {
                var detailsData = new TblAdviceTemplateDetails()
                {
                    Id = Guid.NewGuid(),
                    AdviceTemplateId = adviceTemplate.Id,
                    Advice = item
                };
                templateDetails.Add(detailsData);
            }
            var saveDetails = await _advice.SaveAdviceTemplateDetails(templateDetails);
            await JS.InvokeVoidAsync("onInitTable", "mainTable-advice", masterData);
            templateDetails = new List<TblAdviceTemplateDetails>();
            adviceTemplate = new();
            templateListData = await _advice.GetAdviceTemplateData();
            await JS.InvokeVoidAsync("showAlert", $"{saveOrUpdateContent} Successful", $"Record has been successfully {saveOrUpdateContent}.", "success", "swal-success");
            StateHasChanged();
        }
        [JSInvokable("onDataDelete")]
        public async Task onDataDelete(Guid id)
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("showDeleteConfirmation", "Delete", "Are you sure you want to delete this record?");

            if (isConfirmed)
            {

                if (id != Guid.Empty)
                {
                    var Sql = $"DELETE FROM TblAdviceTemplate WHERE Id = '{id}'";
                    var isDelete = await _advice.DeleteAdviceTemplete(Sql);
                    if (isDelete == true)
                    {
                        var SqlDetails = $"DELETE FROM TblAdviceTemplateDetails WHERE AdviceTemplateId = '{id}'";
                        var isDeleteDetails = await _advice.DeleteAdviceDetails(SqlDetails);
                        templateListData = await _advice.GetAdviceTemplateData();
                        await JS.InvokeVoidAsync("showAlert", "Delete Successful", "Record has been successfully deleted.", "success", "swal-danger");
                        saveOrUpdateContent = "Save";
                        StateHasChanged();
                    }
                }
            }

        }
        [JSInvokable("onDataEdit")]
        public async Task onDataEdit(Guid id)
        {
            try
            {
                var Sql = $"SELECT *  FROM TblAdviceTemplate WHERE Id = '{id}'";
                if (id != Guid.Empty)
                {
                    adviceTemplate = await _advice.GetTemplateById(Sql);

                    var SqlDetails = $"SELECT * FROM TblAdviceTemplateDetails WHERE AdviceTemplateId = '{id}'";
                    var detailsData = new List<TblAdviceTemplateDetails>();
                    detailsData = await _advice.GetTemplateDetailsById(SqlDetails);
                    await JS.InvokeVoidAsync("GeneTable", "mainTable-advice", masterData, detailsData);
                    StateHasChanged();

                }
                saveOrUpdateContent = "Update";
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        [JSInvokable]
        public async void ChangeAdviceData(Guid id)
        {
            var SqlDetails = $"SELECT * FROM TblAdviceTemplateDetails WHERE AdviceTemplateId = '{id}'";
            var detailsData = new List<TblAdviceTemplateDetails>();
            detailsData = await _advice.GetTemplateDetailsById(SqlDetails);
            if (detailsData != null)
            {
                await JS.InvokeVoidAsync("GeneTable", "mainTable-advice", masterData, detailsData);
                StateHasChanged();
            }

        }
    }
}
