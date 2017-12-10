using System;
using System.Collections.Specialized;
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
        /// 印刷ジョブがタイムアウトになる秒数
        /// </summary>
        private int JobTimeOutSecound = 20;
        /// <summary>
        /// 出力先プリンタ名
        /// </summary>
        private string DefaultPrinterName;

        /// <summary>
        /// 生成部、コンフィグセクション指定読み込み
        /// </summary>
        /// <param name="sectionName"></param>
        public PdfPrinter(string sectionName = "PdfPrintSettings")
        {
            var settings = ConfigurationManager.GetSection(sectionName) as NameValueCollection;
            if (settings == null)
                throw new ArgumentException(sectionName + ":設定ファイル内");

            // AdobeReaderPath
            this.AdobeReaderPath = settings[nameof(AdobeReaderPath)];
            if (string.IsNullOrEmpty(this.AdobeReaderPath))
                throw new ArgumentException(nameof(AdobeReaderPath) + "が設定されていません");
            if (!File.Exists(this.AdobeReaderPath))
                throw new FileNotFoundException(this.AdobeReaderPath);

            // DefaultPrinterName
            this.DefaultPrinterName = settings[nameof(DefaultPrinterName)]
                ?? new PrintDocument().PrinterSettings.PrinterName;

            // JobTimeOutSecound
            string _JobTimeOutSecound = settings[nameof(JobTimeOutSecound)];
            if (!string.IsNullOrEmpty(_JobTimeOutSecound))
            {
                if (!int.TryParse(_JobTimeOutSecound, out this.JobTimeOutSecound))
                    throw new ArgumentException(nameof(JobTimeOutSecound) + "には整数値を設定してください");
            }
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
            foreach (var i in Enumerable.Range(0, this.JobTimeOutSecound))
            {
                var hasPrintPdfName = que.GetPrintJobInfoCollection().Any(x => x.Name.EndsWith(pdfFileName));

                // キューに対象ジョブがないので終了
                if (!hasPrintPdfName) return;

                Thread.Sleep(1000);
            }

            throw new TimeoutException("印刷を開始できませんでした");
        }

        /// <summary>
        /// 印刷ジョブ終了を監視する
        /// </summary>
        private void FinishPrintob(string pdfFileName, PrintQueue que)
        {
            foreach (var i in Enumerable.Range(0, this.JobTimeOutSecound))
            {
                var hasPrintPdfName = que.GetPrintJobInfoCollection().Any(x => x.Name.EndsWith(pdfFileName));

                // キューに対象ジョブがないので終了
                if (!hasPrintPdfName) return;

                Thread.Sleep(1000);
            }

            throw new TimeoutException("印刷処理がタイムアウトしました");
        }
    }
}
