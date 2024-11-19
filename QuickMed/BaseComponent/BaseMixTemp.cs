using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;
using System.Text.Json;

namespace QuickMed.BaseComponent
{
    public class BaseMixTemp : ComponentBase
    {
        [Inject]
        public IMixTemp _mix { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }


        public TblMixTemplate model = new();
        public DrugMedicine brnad = new();
        public TblDose dose = new();
        public IEnumerable<TblMixTemplate>? models { get; set; }
        public IEnumerable<DrugMedicine>? brands { get; set; }
        public IEnumerable<TblDose>? doses { get; set; }

        protected override async Task OnInitializedAsync()
        {
            models = await App.Database.GetTableRowsAsync<TblMixTemplate>("TblMixTemplate");
            //brands = await App.Database.GetTableRowsAsync<DrugMedicine>("DrugMedicine");
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

        public async Task AddNewPlusBtn()
        {
            try
            {
                var result = await JS.InvokeAsync<object>("AddNewPlusBtn");
                if (result is not null)
                {
                    var jsonString = result.ToString();


                    var treatments = JsonSerializer.Deserialize<List<TreatmentPopVM>>(jsonString);

                    await JS.InvokeVoidAsync("populateMixTempTable", treatments, "MixTempTbl");

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
