using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseTreatmentTemp : ComponentBase
    {
        [Inject]
        public ITeatmentTemp _teatmentTemp { get; set; }
        public TblTreatmentTemplate treatmentTemplate = new();
        public List<TblTreatmentTempDetails> templateDetails = new List<TblTreatmentTempDetails>();
        public TblTreatmentTempDetails details = new TblTreatmentTempDetails();
        public TblAdviceTemplate adviceTemplate = new();
        public List<TblAdviceTemplateDetails> adviceDetails = new List<TblAdviceTemplateDetails>();
        public TblAdviceTemplateDetails TblAdviceTemplate = new();

        [Inject]
        public IJSRuntime JS { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await JS.InvokeVoidAsync("makeSelect2", true);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("setupEditableTable", "TretmentTmpTbl", null);
                await JS.InvokeVoidAsync("setupEditableTable", "TretmentTmpAdviceTbl", "add_Advice");
                await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");
            }
        }
        public async Task OsudAddbtn()
        {
            try
            {
                var data = await JS.InvokeAsync<dynamic>("OsudAddbtn");

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
