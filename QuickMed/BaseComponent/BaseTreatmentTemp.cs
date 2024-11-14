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
                // Retrieve data from JavaScript
                var result = await JS.InvokeAsync<object>("OsudAddbtn");

                // Check if result is an array; if not, wrap it in an array
                object[] dataArray;
                if (result is object[] array)
                {
                    dataArray = array; // Already an array
                }
                else
                {
                    dataArray = new object[] { result }; // Wrap single object in an array
                }

                // Pass the data array to JavaScript
                await JS.InvokeVoidAsync("populateTreatmentTable", dataArray);




            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
