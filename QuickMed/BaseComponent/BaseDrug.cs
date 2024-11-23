using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.DB;
using QuickMed.Interface;

namespace QuickMed.BaseComponent
{
    public class BaseDrug : ComponentBase
    {
        [Inject]
        public IMixTemp _mix { get; set; }
        [Inject]
        public IDrug _drg { get; set; }
        public DotNetObjectReference<BaseDrug> ObjectReferences { get; private set; }
        public List<DrugMedicine> Brands = new List<DrugMedicine>();

        [Inject]
        public IJSRuntime JS { get; set; }


        protected override async Task OnInitializedAsync()
        {
            ObjectReferences = DotNetObjectReference.Create(this);
            Brands = new();
            Brands = await _mix.GetAllMedicine();

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                await InitializeJS();
            }
        }

        protected async Task InitializeJS()
        {
            await JS.InvokeVoidAsync("setInstanceReferenceForAll", ObjectReferences);
            await JS.InvokeVoidAsync("makeSelect2Custom", "select2C", "GetMedicines", 3);
        }

        [JSInvokable("GetMedicines")]
        public Task<List<DrugMedicine>> LoadMedicines(string search)
        {
            var filtered = string.IsNullOrEmpty(search)
                ? Brands
                : Brands.Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return Task.FromResult(filtered);
        }
    }
}
