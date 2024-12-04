using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickMed.Interface;

namespace QuickMed.BaseComponent.Modals
{
    public class BasePresPreview : ComponentBase
    {
        public DotNetObjectReference<BasePresPreview> ObjectReference { get; private set; }
        [Parameter]
        public PrescriptionViewModel PrescriptionViewModelss { get; set; }
        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public IPrinterMac Printer { get; set; }

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
                var content = await JS.InvokeAsync<string>("getPrintContent");

                // Ensure the content is valid before proceeding
                if (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine("No content found to print.");
                    return;
                }

                // Pass the content to the printer for printing
                Printer.PrintPage(content);
                // JS.InvokeVoidAsync("printModalContentForMac");
            }
        }


    }
}
