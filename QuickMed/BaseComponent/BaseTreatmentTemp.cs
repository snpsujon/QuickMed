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
        public List<TblBrand> Brands = new List<TblBrand>();
        public List<TblDose> Dose = new List<TblDose>();
        public List<TblDuration> Duration = new List<TblDuration>();
        public List<TblInstruction> Instructions = new List<TblInstruction>();
        public List<TblAdviceTemplate> adviceMasters = new List<TblAdviceTemplate>();

        [Inject]
        public IJSRuntime JS { get; set; }


        protected override async Task OnInitializedAsync()
        {
            //await JS.InvokeVoidAsync("makeSelect2", true);
            Brands = new();
            Brands = await App.Database.GetTableRowsAsync<TblBrand>("TblBrand");
            Dose = new();
            Dose = await App.Database.GetTableRowsAsync<TblDose>("TblDose");
            Duration = new();
            Duration = await App.Database.GetTableRowsAsync<TblDuration>("TblDuration");
            Instructions = new();
            Instructions = await App.Database.GetTableRowsAsync<TblInstruction>("TblInstruction");
            adviceMasters = new();
            adviceMasters = await App.Database.GetTableRowsAsync<TblAdviceTemplate>("TblAdviceTemplate");
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await JS.InvokeVoidAsync("setupEditableTable", "TretmentTmpTbl", null);
                await JS.InvokeVoidAsync("setupEditableTable", "TretmentTmpAdviceTbl", "add_Advice");
                await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");
                await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpAdviceTbl");
                await JS.InvokeVoidAsync("makeSelect2", true);
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
