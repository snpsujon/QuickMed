using CoreGraphics;
using QuickMed.Interface;
using QuickMed.MacOS;  // Replace with your actual namespace
using UIKit;
using WebKit;


[assembly: Dependency(typeof(MacPrinter))]
namespace QuickMed.MacOS
{
    public class MacPrinter : IPrinterMac
    {
        public void PrintPage(string content)
        {
            // Create a WKWebView to render HTML content
            var wkWebView = new WKWebView(new CGRect(0, 0, 1000, 1000), new WKWebViewConfiguration());
            wkWebView.LoadHtmlString(content, null);


            wkWebView.NavigationDelegate = new NavigationDelegateForPrinting((webView) =>
            {
                // Create a UIPrintInteractionController for printing
                var printController = UIPrintInteractionController.SharedPrintController;


                // Set up print info
                var printInfo = UIPrintInfo.PrintInfo;
                printInfo.OutputType = UIPrintInfoOutputType.General;  // Set output type
                printInfo.JobName = "Print Job";  // Set a name for the print job
                printController.PrintInfo = printInfo;


                // Use the WKWebView's print formatter
                printController.PrintFormatter = webView.ViewPrintFormatter;


                // Start the print dialog
                printController.Present(true, (handler, completed, error) =>
                {
                    if (completed)
                    {
                        Console.WriteLine("Print job completed successfully.");
                    }
                    else if (error != null)
                    {
                        Console.WriteLine($"Error printing: {error.LocalizedDescription}");
                    }
                });
            });
        }

    }


    public class NavigationDelegateForPrinting : WKNavigationDelegate
    {
        private readonly Action<WKWebView> _onPageLoadComplete;


        public NavigationDelegateForPrinting(Action<WKWebView> onPageLoadComplete)
        {
            _onPageLoadComplete = onPageLoadComplete;
        }


        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            // Invoke the callback once the web view finishes loading
            _onPageLoadComplete?.Invoke(webView);
        }
    }
}
