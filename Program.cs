using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string text = File.ReadAllText("TestDocument.txt");
            PdfDocument doc = new PdfDocument();
            PdfSection section = doc.Sections.Add();
            PdfPageBase page = section.Pages.Add();
            PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 11);
            PdfStringFormat format = new PdfStringFormat();
            format.LineSpacing = 20f;
            PdfBrush brush = PdfBrushes.Black;
            PdfTextWidget textWidget = new PdfTextWidget(text, font, brush);
            float y = 0;
            PdfTextLayout textLayout = new PdfTextLayout();
            textLayout.Break = PdfLayoutBreakType.FitPage;
            textLayout.Layout = PdfLayoutType.Paginate;
            RectangleF bounds = new RectangleF(new PointF(0, y), page.Canvas.ClientSize);
            textWidget.StringFormat = format;
            textWidget.Draw(page, bounds, textLayout);
            doc.SaveToFile("TxtToPDf.pdf", FileFormat.PDF);
            */





            PdfDocument pdf = new PdfDocument(@"C:\Anil\TestFile.pdf");

            //Add a page  
           PdfPageBase page = pdf.Pages.Add();

            //Insert text  
            page.Canvas.DrawString("This document is protected with user password ", new PdfFont(PdfFontFamily.Helvetica, 13f), PdfBrushes.Black, PointF.Empty);

            //Specify encryption level (algorithm)  
            pdf.Security.KeySize = PdfEncryptionKeySize.Key128Bit;

            //Add owner password  
            pdf.Security.UserPassword = "ownerpassword";

            //Save the document  
            pdf.SaveToFile(@"c:\anil\Userpassword.pdf");
        }
    }
}
