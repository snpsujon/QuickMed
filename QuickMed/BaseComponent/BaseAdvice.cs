using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuickMed.BaseComponent
{
    public class BaseAdvice:ComponentBase
    {
        [Inject]
        public IAdvice _advice { get; set; }
        public TblAdviceTemplate adviceTemplate = new();
        public string saveOrUpdateContent { get; set; } = "Save";
        public List<TblAdviceTemplateDetails> templateDetails = new List<TblAdviceTemplateDetails>();
        public IEnumerable<TblAdviceMaster>? masterData { get; set; }
        public IEnumerable<TblAdviceTemplate> templateListData { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        protected override async  Task OnInitializedAsync()
        {            
            masterData = await _advice.GetAdviceMasterData();
            await JS.InvokeVoidAsync("onInitTable", "mainTable-advice",masterData);
            await JS.InvokeVoidAsync("initializeButtonClick",masterData);
            templateListData = await _advice.GetAdviceTemplateData();
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
          var tableSelectedValue=   await JS.InvokeAsync<List<string>>("GetTableData", "mainTable-advice");
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
            if (adviceTemplate.Id == Guid.Empty)
            {
                adviceTemplate.Id = Guid.NewGuid();
                if(adviceTemplate.AdviceTemplateName == null)
                {
                    adviceTemplate.AdviceTemplateName = await JS.InvokeAsync<string>("GenerateAdviceTemplateName");
                }
                var isSave = await _advice.SaveAdviceTemplate(adviceTemplate);
            }
            else
            {
                var updateTemplate = await _advice.UpdateAdviceTemplate(adviceTemplate);
                if(updateTemplate == true)
                {
                    var SqlDetails = $"DELETE FROM TblAdviceTemplateDetails WHERE AdviceTemplateId = '{adviceTemplate.Id}'";
                    var isDeleteDetails = await _advice.DeleteAdviceDetails(SqlDetails);
                }

            }
            var saveDetails = await _advice.SaveAdviceTemplateDetails(templateDetails);
            await JS.InvokeVoidAsync("onInitTable", "mainTable-advice", masterData);
            templateDetails = new List<TblAdviceTemplateDetails>();
            adviceTemplate = new();
            templateListData = await _advice.GetAdviceTemplateData();
            StateHasChanged();
        }
       protected async Task onDataDelete(Guid id)
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
                        StateHasChanged();
                    }
                }
            }
            
        }
        protected async Task onDataEdit(Guid id)
        {
            try
            {
                var Sql = $"SELECT *  FROM TblAdviceTemplate WHERE Id = '{id}'";
                if (id != Guid.Empty)
                {
                    adviceTemplate = await _advice.GetTemplateById(Sql);

                    var SqlDetails = $"SELECT * FROM TblAdviceTemplateDetails WHERE AdviceTemplateId = '{id}'";
                   templateDetails = await _advice.GetTemplateDetailsById(SqlDetails);
                    await JS.InvokeVoidAsync("GeneTable", "mainTable-advice",masterData,templateDetails);
                    StateHasChanged();

                }
                saveOrUpdateContent = "Update";
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
