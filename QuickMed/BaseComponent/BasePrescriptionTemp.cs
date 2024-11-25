using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BasePrescriptionTemp : ComponentBase
    {
        [Inject]
        public IMixTemp _mix { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }

        public DotNetObjectReference<BasePrescriptionTemp> ObjectReference { get; private set; }
        private BaseFavouriteDrugTemp _favdrag { get; set; } = new BaseFavouriteDrugTemp();
        public DotNetObjectReference<BasePrescriptionTemp> ObjectReferenceForFavDrag { get; private set; }
        public IEnumerable<TblFavouriteDrugTemplate>? FavDrugTemps { get; set; }
        public IEnumerable<TblTreatmentTemplate>? treatmentTemps { get; set; }
        public IEnumerable<TblPerceptionTemplate>? PrescTemps { get; set; }
        public List<DrugMedicine> Brands = new List<DrugMedicine>();
        public List<TblDose> Dose = new List<TblDose>();
        public List<TblInstruction> Instructions = new List<TblInstruction>();
        public List<TblDuration> Duration = new List<TblDuration>();

        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            FavDrugTemps = await App.Database.GetTableRowsAsync<TblFavouriteDrugTemplate>("TblFavouriteDrugTemplate");
            treatmentTemps = await App.Database.GetTableRowsAsync<TblTreatmentTemplate>("TblTreatmentTemplate");
            PrescTemps = await App.Database.GetTableRowsAsync<TblPerceptionTemplate>("TblPerceptionTemplate");
            Brands = new();
            Brands = await _mix.GetAllMedicine();
            Dose = new();
            Dose = await App.Database.GetTableRowsAsync<TblDose>("TblDose");
            Instructions = new();
            Instructions = await App.Database.GetTableRowsAsync<TblInstruction>("TblInstruction");
            Duration = new();
            Duration = await App.Database.GetTableRowsAsync<TblDuration>("TblDuration");

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
                await InitializeJS();
            }
        }

        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("OnChangeEvent", "presDrugTempSelect", "LoadFavDrugTemplate", ObjectReference);
            await JS.InvokeVoidAsync("initializeQuill", "#editors_pres");

        }

        //[JSInvokable("LoadFavDrugTemplate")]
        //public async Task LoadFavDrugTemplate(string selectedData)
        //{
        //    try
        //    {
        //        if (selectedData is not null)
        //        {
        //            FavouriteDrugTempVM favDrugTemp = await _pres.GetFavDrugbyId(Guid.Parse(selectedData));
        //            TreatmentPopVM treatmentPopVM = new TreatmentPopVM()
        //            {
        //                brand = new BrandVM()
        //                {
        //                    text = favDrugTemp.BrandName,
        //                    value = favDrugTemp.BrandId.ToString()
        //                },
        //                dose = new DoseVM()
        //                {
        //                    text = favDrugTemp.DoseName,
        //                    value = favDrugTemp.DoseId.ToString()
        //                },
        //                duration = new DurationVM()
        //                {
        //                    text = favDrugTemp.DurationName,
        //                    value = favDrugTemp.DurationId.ToString()
        //                },
        //                instruction = new InstructionVM()
        //                {
        //                    text = favDrugTemp.InstructionName,
        //                    value = favDrugTemp.InstructionId.ToString()
        //                }
        //            };
        //            var result = await JS.InvokeAsync<object>("pushtoPrescription", treatmentPopVM);
        //            if (result is not null)
        //            {
        //                var jsonString = result.ToString();
        //                var treatments = JsonSerializer.Deserialize<List<TreatmentPopVM>>(jsonString);
        //                await JS.InvokeVoidAsync("populateTreatmentTable", treatments, "TretmentTmpTbl");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

    }
}
