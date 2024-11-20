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
        public IEnumerable<TblTreatmentTemplate>? treatmentTemps { get; set; }

        public IEnumerable<TblFavouriteDrugTemplate>? FavDrugTemps { get; set; }

        public IEnumerable<TblPerceptionTemplate>? PrescTemps { get; set; }

        public IEnumerable<TblBrandTemplate>? BrandTemps { get; set; }

        public IEnumerable<TblDoseTemplate>? DoseTemps { get; set; }

        public IEnumerable<TblInstructionTemplate> InsTemps { get; set; }

        public IEnumerable<TblDurationTemplate> DuraTionTemps { get; set; }

        public IEnumerable<TblAdviceTemplate> AdviceTemps { get; set; }

        public IEnumerable<TblRefferTemplate> RefferTemps { get; set; }


        protected override async Task OnInitializedAsync()
        {
            patients = await App.Database.GetTableRowsAsync<TblPatient>("TblPatient");
            brands = await _mix.GetAllMedicine();
            doses = await App.Database.GetTableRowsAsync<TblDose>("TblDose");

            treatmentTemps = await App.Database.GetTableRowsAsync<TblTreatmentTemplate>("TblTreatmentTemplate");

            FavDrugTemps = await App.Database.GetTableRowsAsync<TblFavouriteDrugTemplate>("TblFavouriteDrugTemplate");

            PrescTemps = await App.Database.GetTableRowsAsync<TblPerceptionTemplate>("TblPerceptionTemplate");

            BrandTemps = await App.Database.GetTableRowsAsync<TblBrandTemplate>("TblBrandTemplate");

            DoseTemps = await App.Database.GetTableRowsAsync<TblDoseTemplate>("TblDoseTemplate");

            InsTemps = await App.Database.GetTableRowsAsync<TblInstructionTemplate>("TblInstructionTemplate");

            DuraTionTemps = await App.Database.GetTableRowsAsync<TblDurationTemplate>("TblDurationTemplate");

            AdviceTemps = await App.Database.GetTableRowsAsync<TblAdviceTemplate>("TblAdviceTemplate");

            RefferTemps = await App.Database.GetTableRowsAsync<TblRefferTemplate>("TblRefferTemplate");
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
