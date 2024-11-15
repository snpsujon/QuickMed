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
            if (adviceTemplate.Id == Guid.Empty)
            {
                adviceTemplate.Id = Guid.NewGuid();
                var isSave = await _advice.SaveAdviceTemplate(adviceTemplate);
                if (isSave == true)
                {
                    foreach(var item in tableSelectedValue)
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
                    templateDetails= new List<TblAdviceTemplateDetails>();
                    adviceTemplate = new();
                    templateListData = await _advice.GetAdviceTemplateData();
                    StateHasChanged();
                }
            }
        }
       protected async Task onDataDelete(Guid id)
        {
            var Sql = $"DELETE FROM TblAdviceTemplate WHERE Id = '{id}'";
            if(id != Guid.Empty)
            {
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
        [JSInvokable]
        public static Task OnDataDelete(int id)
        {
            Console.WriteLine($"Deleting data with ID: {id}");
            // Perform your deletion logic here
            return Task.CompletedTask;
        }
    }
}
