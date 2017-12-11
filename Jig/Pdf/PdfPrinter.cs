using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Threading;

namespace Jig.Pdf
{
    /// <summary>
    /// PDF印刷
    /// </summary>
    public class PdfPrinter
    {
        /// <summary>
        /// AdobeReaderパス
        /// </summary>
        private string AdobeReaderPath;
        /// <summary>
        /// 出力先プリンタ名
        /// </summary>
        private string DefaultPrinterName;
        /// <summary>
        /// 印刷ジョブがタイムアウトになる秒数(ms)
        /// </summary>
        private int JobTimeOutMiliSecounds;

        /// <summary>
        /// 生成部、コンフィグセクション指定読み込み
        /// </summary>
        /// <param name="sectionName"></param>
        public static PdfPrinter GetInsance(string sectionName = "PdfPrinterSettings")
        {

            var settings = (PdfPrinterSettings)ConfigurationManager.GetSection(sectionName);
            if (settings == null)
                throw new ArgumentException(sectionName + ":設定ファイル内");

            return new PdfPrinter
            {
                AdobeReaderPath = settings.AdobeReaderPath,
                DefaultPrinterName = settings.AdobeReaderPath
                  ?? new PrintDocument().PrinterSettings.PrinterName,
                JobTimeOutMiliSecounds = settings.FileRetryWaitMiliSeconds,
            };
        }

        public void PrintPdf(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var printProcess = new Process();
            printProcess.StartInfo.FileName = this.AdobeReaderPath;
            printProcess.StartInfo.Verb = "open";
            printProcess.StartInfo.Arguments = $@" /t /n /h /s /l ""{filePath}"" ""{this.DefaultPrinterName}""";
            printProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            printProcess.StartInfo.CreateNoWindow = true;

            try
            {
                // ジョブ監視用キュー
                var que = new LocalPrintServer().GetPrintQueue(this.DefaultPrinterName);

                // 印刷
                printProcess.Start();

                // ジョブ監視
                StartPrintob(this.DefaultPrinterName, que);
                FinishPrintob(this.DefaultPrinterName, que);
            }
            finally
            {
                printProcess.Dispose();
            }
        }

        /// <summary>
        /// 印刷ジョブ開始を監視する
        /// </summary>
        private void StartPrintob(string pdfFileName, PrintQueue que)
        {
            foreach (var i in Enumerable.Range(0, this.JobTimeOutMiliSecounds / 100))
            {
                var hasPrintPdfName = que.GetPrintJobInfoCollection().Any(x => x.Name.EndsWith(pdfFileName));

                // キューに対象ジョブがないので終了
                if (!hasPrintPdfName) return;

                Thread.Sleep(100);
            }

            throw new TimeoutException("印刷を開始できませんでした");
        }

        /// <summary>
        /// 印刷ジョブ終了を監視する
        /// </summary>
        private void FinishPrintob(string pdfFileName, PrintQueue que)
        {
            foreach (var i in Enumerable.Range(0, this.JobTimeOutMiliSecounds / 100))
            {
                var hasPrintPdfName = que.GetPrintJobInfoCollection().Any(x => x.Name.EndsWith(pdfFileName));

                // キューに対象ジョブがないので終了
                if (!hasPrintPdfName) return;

                Thread.Sleep(100);
            }

            throw new TimeoutException("印刷処理がタイムアウトしました");
        }
    }
}
