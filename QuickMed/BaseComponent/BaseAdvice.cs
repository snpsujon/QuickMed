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
            templateListData = await _advice.GetAdviceTemplateData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("setupEditableTable", "mainTable-advice", "but_add");
                await JS.InvokeVoidAsync("makeTableDragable", "mainTable-advice");
                await JS.InvokeVoidAsync("setupEditableTableWithoutButton", "mainTable-advice");
                await JS.InvokeVoidAsync("makeSelect2", true);
            }
        }
       

    }
}
