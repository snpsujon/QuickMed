using Microsoft.AspNetCore.Components;
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

        protected override async Task OnInitializedAsync()
        {
            await JS.InvokeVoidAsync("onInitTable", "mainTable-advice", tblCCTemplate);
            await JS.InvokeVoidAsync("initializeButtonClick", tblCCTemplate);
            tblCCTemplates = await _cCTemp.GetCCTempData();
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
    }
}
