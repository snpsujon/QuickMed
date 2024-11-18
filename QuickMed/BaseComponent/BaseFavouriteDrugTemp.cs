using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseFavouriteDrugTemp : ComponentBase
    {
        [Inject]
        public IFavouriteDrug _drug { get; set; }


        public TblFavouriteDrugTemplate model = new();
        public IEnumerable<TblFavouriteDrugTemplate>? models { get; set; }


        [Inject]
        public IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeDataTable();
            models = await _drug.GetAsync();

        }

        private async Task InitializeDataTable()
        {
            await JS.InvokeVoidAsync("makeDataTable", "datatable-dxTemp");
        }
    }
}
