using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System.Text.Json;

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

        [Inject]
        public ITeatmentTemp _teatmentTemp { get; set; }


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

        //public IEnumerable<TblAdviceTemplate> adviceMasters { get; set; }

        public IEnumerable<TblRefferTemplate> RefferTemps { get; set; }

        public string tblid { get; set; }

        public DotNetObjectReference<BasePrescription> ObjectReference { get; private set; }

        public List<DrugMedicine> Brands = new List<DrugMedicine>();
        public List<TblDose> Dose = new List<TblDose>();
        public List<TblDuration> Duration = new List<TblDuration>();
        public List<TblInstruction> Instructions = new List<TblInstruction>();
        public List<TblAdviceTemplateDetails> adviceDetails = new List<TblAdviceTemplateDetails>();
        public List<TblAdviceTemplate> adviceMasters = new List<TblAdviceTemplate>();
        public int SelectedDays { get; set; } = 0;
        public DateTime NextMeetingDate { get; set; } = DateTime.Now;


        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            treatmentTemps = await App.Database.GetTableRowsAsync<TblTreatmentTemplate>("TblTreatmentTemplate");
            FavDrugTemps = await App.Database.GetTableRowsAsync<TblFavouriteDrugTemplate>("TblFavouriteDrugTemplate");
            PrescTemps = await App.Database.GetTableRowsAsync<TblPerceptionTemplate>("TblPerceptionTemplate");
            adviceMasters = new();
            adviceMasters = await App.Database.GetTableRowsAsync<TblAdviceTemplate>("TblAdviceTemplate");
            RefferTemps = await App.Database.GetTableRowsAsync<TblRefferTemplate>("TblRefferTemplate");
            Brands = new();
            Brands = await _mix.GetAllMedicine();
            Dose = new();
            Dose = await App.Database.GetTableRowsAsync<TblDose>("TblDose");
            Duration = new();
            Duration = await App.Database.GetTableRowsAsync<TblDuration>("TblDuration");
            Instructions = new();
            Instructions = await App.Database.GetTableRowsAsync<TblInstruction>("TblInstruction");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
                await InitializeJS();
            }
        }

        protected async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        }

        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            await JS.InvokeVoidAsync("makeSelect2", true);
            await JS.InvokeVoidAsync("makeSelect2Custom", "select2C", "GetMedicines", 3);
            await JS.InvokeVoidAsync("setupEditableTable", "TretmentTmpAdviceTbl", "add_Advice");
            await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");
            await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpAdviceTbl");
            await JS.InvokeVoidAsync("OnChangeEvent", "adviceSelect", "AdviceChange", ObjectReference);
            await JS.InvokeVoidAsync("OnChangeEvent", "nxtMeetDateSelect", "NextMeetDateChange", ObjectReference);


            //await JS.InvokeVoidAsync("setupEditableTable", "MixTempTbl", "add_MixTemp");
            //await JS.InvokeVoidAsync("makeTableDragable", "TretmentTmpTbl");

        }
        public void Dispose()
        {
            ObjectReference?.Dispose();
        }

        [JSInvokable("GetMedicines")]
        public Task<List<DrugMedicine>> LoadMedicines(string search)
        {
            var filtered = string.IsNullOrEmpty(search)
                ? Brands
                : Brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return Task.FromResult(filtered);
        }


        public async Task OsudAddbtn()
        {
            try
            {
                var result = await JS.InvokeAsync<object>("OsudAddbtn");
                if (result is not null)
                {
                    var jsonString = result.ToString();

                    var treatments = JsonSerializer.Deserialize<List<TreatmentPopVM>>(jsonString);
                    await JS.InvokeVoidAsync("populateTreatmentTable", treatments, "TretmentTmpTbl");
                    await JS.InvokeVoidAsync("populateTreatmentTable", treatments, "TretmentTmpTbl");

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [JSInvokable]
        public async Task AdviceChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    adviceDetails = await _teatmentTemp.GetAdviceDataById(selectedData);
                    await JS.InvokeVoidAsync("populateAdviceTable", adviceDetails, "TretmentTmpAdviceTbl");

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [JSInvokable]
        public async Task NextMeetDateChange(string selectedData)
        {
            try
            {
                //var selectedData = await JS.InvokeAsync<string>("getAdviceValue");
                if (selectedData is not null)
                {
                    await JS.InvokeVoidAsync("changeNxtDatebyVal", selectedData);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task OnSelectedDaysChanged()
        {
            string value = "2";
            if (int.TryParse(value, out var days))
            {
                SelectedDays = days;
                NextMeetingDate = DateTime.Now.AddDays(SelectedDays);
            }
            else
            {
                SelectedDays = 0;
                NextMeetingDate = DateTime.Now;
            }
            StateHasChanged();
        }

        public async Task AddaRow(string tblid)
        {
            await JS.InvokeVoidAsync("myrowAddNew", tblid, false);
        }


        public async Task UpdateBMI()
        {
            await JS.InvokeVoidAsync("updateBMI");
        }





    }
}
