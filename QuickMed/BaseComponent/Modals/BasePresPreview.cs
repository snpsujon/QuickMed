using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace QuickMed.BaseComponent.Modals
{
    public class BasePresPreview : ComponentBase
    {
        public DotNetObjectReference<BasePresPreview> ObjectReference { get; private set; }
        [Parameter]
        public PrescriptionViewModel PrescriptionViewModelss { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ObjectReference = DotNetObjectReference.Create(this);

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }



        public async void PreviewBtn()
        {
            var platform = Microsoft.Maui.Devices.DeviceInfo.Platform;

            if (platform == Microsoft.Maui.Devices.DevicePlatform.WinUI)
            {
                // Call the JavaScript function for Windows
                JS.InvokeVoidAsync("printModalContent");
            }
            else if (platform == Microsoft.Maui.Devices.DevicePlatform.MacCatalyst)
            {
                // Call a different JavaScript function for macOS
                JS.InvokeVoidAsync("printModalContentForMac");
            }
        }


    }
}
