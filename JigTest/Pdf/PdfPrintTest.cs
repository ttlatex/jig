using Jig.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JigTest.Pdf
{
    public class PdfPrintTest
    {
        public void PrintT()
        {
            string pdfPath = @"";

            new PdfPrinter().PrintPdf(pdfPath);
        }
    }
}
