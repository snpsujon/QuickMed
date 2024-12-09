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

        private static readonly Dictionary<string, string> banglaTimeDictionary = new Dictionary<string, string>
        {
            { "00:00", "রাত ১২টা" },
            { "01:00", "রাত ১টা" },
            { "02:00", "রাত ২টা" },
            { "03:00", "রাত ৩টা" },
            { "04:00", "রাত ৪টা" },
            { "05:00", "রাত ৫টা" },
            { "06:00", "সকাল ৬টা" },
            { "07:00", "সকাল ৭টা" },
            { "08:00", "সকাল ৮টা" },
            { "09:00", "সকাল ৯টা" },
            { "10:00", "সকাল ১০টা" },
            { "11:00", "সকাল ১১টা" },
            { "12:00", "দুপুর ১২টা" },
            { "13:00", "দুপুর ১টা" },
            { "14:00", "দুপুর ২টা" },
            { "15:00", "বিকেল ৩টা" },
            { "16:00", "বিকেল ৪টা" },
            { "17:00", "বিকেল ৫টা" },
            { "18:00", "সন্ধ্যা ৬টা" },
            { "19:00", "সন্ধ্যা ৭টা" },
            { "20:00", "রাত ৮টা" },
            { "21:00", "রাত ৯টা" },
            { "22:00", "রাত ১০টা" },
            { "23:00", "রাত ১১টা" }
        };


#if MACCATALYST
        [Inject]
        public IPrinterMac Printer { get; set; }
#endif



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

        public static string GetBanglaTime(string? time)
        {
            if (time == null)
                return null;
            // Check if the key exists in the dictionary
            if (banglaTimeDictionary.TryGetValue(time, out string banglaTime))
            {
                return banglaTime;
            }

            return null; // Return null if the time is not found
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
#if MACCATALYST
                Printer.PrintPage(content);
#endif
                // Pass the content to the printer for printing

                // JS.InvokeVoidAsync("printModalContentForMac");
            }
        }


    }
}
