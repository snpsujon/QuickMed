using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseIXTemp : ComponentBase
    {
        [Inject]
        public IIXTemp _ixTemp { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }



        public TblIXTemplate model = new();
        public IEnumerable<TblIXTemplate>? models { get; set; }


        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        await JS.InvokeVoidAsync("makeDataTable", "datatable-ixTemp");
        //        await JS.InvokeVoidAsync("setupEditableTable", "makeEditable_IxTemp", "but_add_IxTemp");
        //    }
        //}

        //protected override async Task OnInitializedAsync()
        //{
        //    //models = await _ixTemp.GetAsync(); // Load the initial data
        //}

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
            await JS.InvokeVoidAsync("setupEditableTable", "datatable-patientList");
            await JS.InvokeVoidAsync("makeSelect2", false);
        }




    }
}
