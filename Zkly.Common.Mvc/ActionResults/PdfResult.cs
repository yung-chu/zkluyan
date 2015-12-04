using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.HtmlConverter;

namespace Zkly.Common.Mvc.ActionResults
{
    public class PdfResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            //TODO: Sample code
            //Create a pdf document.
            var doc = new PdfDocument();

            var settings = new PdfPageSettings { Orientation = PdfPageOrientation.Portrait };

            //settings.Size = PdfPageSize.A4;
            var format = new PdfHtmlLayoutFormat();
            format.FitToPage = Clip.Height;
            format.Layout = PdfLayoutType.Paginate;

            const string Url = "http://staging.zkluyan.com";

            var thread = new Thread(() =>
            {
                doc.LoadFromHTML(Url, false, true, true, settings, format);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            //Save pdf file.
            doc.SaveToFile("sample.pdf", FileFormat.PDF);
            doc.Close();
        }
    }
}
