using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;
using QuickMed.ViewModels;

namespace QuickMed.BaseComponent
{
    public class BaseDashboard : ComponentBase
    {
        [Inject]
        public IDashboard _dashboard { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        public TblPrescription dose = new();
        public IEnumerable<TblDose>? doses { get; set; }
        public List<DxTempVM> dxs = new List<DxTempVM>();
        public List<BrandDashboardVM> brands = new List<BrandDashboardVM>();
        public TotalDashboardVM dashboards = new();
        public DotNetObjectReference<BaseDashboard> ObjectReference { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReference);
            dashboards = await _dashboard.GetTotalDashboardData();
            dxs = new();
            dxs = await _dashboard.GetTotalDxDashboardData();
            brands = new();
            brands = await _dashboard.GetBrandDashboardData();
            //doses = await App.Database.GetTableRowsAsync<TblDose>("TblDose");
            //doses = await _dashboard.GetMostUsedDXAsync(); // Load the initial data
            await RefreshDataTable();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
            }
        }
        protected async Task RefreshDataTable()
        {

            var tableData = doses?.Select((ds, index) => new[]
                {
                    (index + 1).ToString(), // Serial number starts from 1
                    ds.Name?.ToString() ?? string.Empty, // Note name
                    $@"
                    <div style='display: flex; justify-content: flex-end;'>
                        <i class='dripicons-pencil btn btn-soft-primary dTRowActionBtn' data-id='{ds.Id}' data-method='OnEditClick'></i>
                        <i class='dripicons-trash btn btn-soft-danger dTRowActionBtn' data-id='{ds.Id}' data-method='OnDeleteClick'></i>
                    </div>
                    " // Action buttons
                }).ToArray();

            await JS.InvokeVoidAsync("makeDataTableQ", "datatable-doseTemp", tableData);

        }
    }
}
