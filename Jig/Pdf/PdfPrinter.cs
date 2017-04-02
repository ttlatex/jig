using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Pdf
{
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
        /// 初期化の際にコンフィグセクションを読み込んだか
        /// </summary>
        public bool IsReadSection { private set; get; }

        /// <summary>
        /// 生成部、何も指定しなかった場合コンフィグセクション"PdfPrintSettings"が読み込まれます
        /// </summary>
        public PdfPrinter()
           : this("PdfPrintSettings")
        {
        }

        /// <summary>
        /// 生成部、コンフィグセクション指定読み込み
        /// </summary>
        /// <param name="sectionName"></param>
        public PdfPrinter(string sectionName)
        {
            this.IsReadSection = false;
            var settings = ConfigurationManager.GetSection(sectionName) as NameValueCollection;
            if (settings == null) throw new ArgumentException(sectionName + ":設定ファイル内");
            this.IsReadSection = true;

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
            if (string.IsNullOrEmpty(_JobTimeOutSecound))
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

            var que = new LocalPrintServer().

        }
    }
}
