


namespace Helpers.HtmlToPdf
{
    public class HtmlToPdfConverter
    {
        public HtmlToPdfConverter()
        {

        }

        public async Task<byte[]> HtmlTopdf()
        {
            var renderer = new ChromePdfRenderer(); // Instantiates Chrome Renderer
            var pdf = renderer.RenderHtmlAsPdf("<h1>~Hello World~</h1> Made with IronPDF!");
            return pdf.BinaryData; // Return PDF data as a byte array
        }

    }
}
