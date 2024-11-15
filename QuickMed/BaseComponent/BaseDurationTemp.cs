using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseDurationTemp : ComponentBase
    {
        [Inject]
        public IDurationTemp _duration { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }


        public TblDuration model = new();
        public IEnumerable<TblDuration>? models { get; set; }

        protected override async Task OnInitializedAsync()
        {
            models = await _duration.GetAsync(); // Load the initial data
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeDataTable(); // Initialize JavaScript-based DataTable once the component has rendered
            }
        }

        protected async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-patientList");
        }
    }
}
