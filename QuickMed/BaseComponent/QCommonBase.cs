using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace QuickMed.BaseComponent
{
    public class QCommonBase
    {
        [Inject]
        public IJSRuntime JS { get; set; }
    }
}
