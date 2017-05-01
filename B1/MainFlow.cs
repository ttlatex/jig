using B1.Business;
using B1.Settings;
using Jig.Pdf;
using log4net;
using System;
using System.Collections.Generic;
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
        private PdfPrinter printer;
        private AppSettings appSettings;

        public void start()
        {
            if (!this.Init())
                return;
            this.MainProcess();
        }

        private bool Init()
        {
            this.logger.Info("■処理を開始します");

            try
            {
                this.appSettings = new AppSettings();
                this.appSettings.ThrowArgumetError();
                this.printer = new PdfPrinter();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("初期処理", ex);
                return false;
            }
        }

        private void MainProcess()
        {
            try
            {
                this.logger.Info("データを取得します");
                var pdfData = new ListValueSelector().SelectItems();

                this.logger.Info("PDFの出力を行います");
                var pdfPath = Path.Combine(this.appSettings.OutputFolder, "名前リスト_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                Directory.CreateDirectory(this.appSettings.OutputFolder);
                new PdfCreator(this.appSettings.TemplatePath, pdfPath).OutputPDF(pdfData);


            }
            catch (Exception ex)
            {
                this.logger.Error("主処理", ex);
            }

        }
    }
}
