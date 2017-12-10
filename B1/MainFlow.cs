using B1.Business;
using B1.Settings;
using Jig.Pdf;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1
{
    /// <summary>
    /// メインフローです
    /// </summary>
    public class MainFlow
    {
        private ILog logger = LogManager.GetLogger("InfoLogger");

        public void start()
        {
            try
            {
                this.MainProcess();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void MainProcess()
        {
            this.logger.Info("■処理を開始します");

            var settings = (B1Settings)ConfigurationManager.GetSection("B1Settings");
            var printer = new PdfPrinter();

            this.logger.Info("データを取得します");
            var pdfData = new ListValueSelector().SelectItems();

            this.logger.Info("PDFの出力を行います");
            var pdfPath = Path.Combine(settings.OutputFolder, "名前リスト_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            PdfCreator.OutputPDF(settings.TemplatePath, pdfPath, pdfData);

            this.logger.Info("PDFの出力を行います");
            printer.PrintPdf(pdfPath);

            this.logger.Info("■処理を終了します");
        }
    }
}
