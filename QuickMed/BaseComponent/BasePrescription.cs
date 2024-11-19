using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BasePrescription : ComponentBase
    {
        [Inject]
        public IPrescription _pres { get; set; }
        [Inject]
        public IMixTemp _mix { get; set; }



        [Inject]
        public IJSRuntime JS { get; set; }


        public TblPatient patient = new();
        public DrugMedicine brnad = new();
        public TblDose dose = new();
        public IEnumerable<TblPatient>? patients { get; set; }
        public IEnumerable<DrugMedicine>? brands { get; set; }
        public IEnumerable<TblDose>? doses { get; set; }

        protected override async Task OnInitializedAsync()
        {
            patients = await App.Database.GetTableRowsAsync<TblPatient>("TblPatient");
            brands = await _mix.GetAllMedicine();
            doses = await App.Database.GetTableRowsAsync<TblDose>("TblDose");
        }

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
            await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        }

        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setupEditableTable", "MixTempTbl", "add_MixTemp");
            await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");
            await JS.InvokeVoidAsync("makeSelect2", true);

        }
    }
}
