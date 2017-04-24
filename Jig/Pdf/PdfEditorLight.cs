using iTextSharp.text;
using iTextSharp.text.pdf;
using Jig.Pdf.PdfConstant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Pdf
{
    /// <summary>
    /// ITextSharp操作クラス
    /// </summary>
    public class PdfEditorLight
    {
        /// <summary>
        /// 現在ページ
        /// このプロパティ必要？
        /// </summary>
        public int CurrentPage { get; private set; }
        /// <summary>
        /// 用紙方向
        /// このプロパティ必要？
        /// </summary>
        public PaperOrientation Orientation { get; private set; }
        /// <summary>
        /// 出力PDF
        /// </summary>
        public Document TargetPdf { get; private set; }
        /// <summary>
        /// PDFライター
        /// </summary>
        public PdfWriter TargetPdfWriter { get; private set; }


        #region 生成
        public PdfEditorLight()
        {
            this.CurrentPage = 0;

            if (!File.Exists(@"C:\Windows\Fonts\msgothic.ttc"))
                throw new FileNotFoundException(@"C:\Windows\Fonts\msgothic.ttc");
            if (!File.Exists(@"C:\Windows\Fonts\msmincho.ttc"))
                throw new FileNotFoundException(@"C:\Windows\Fonts\msmincho.ttc");
            if (!File.Exists(@"C:\Windows\Fonts\OCRB.TTF"))
                throw new FileNotFoundException(@"C:\Windows\Fonts\OCRB.TTF");

            // todo フォントの事前読み込み
        }
        #endregion

        /// <summary>
        /// 新規PDF生成
        /// </summary>
        /// <param name="outputFilePath">出力先パス</param>
        /// <param name="templateFilePath">テンプレートファイルパス</param>
        /// <param name="templateTargetPage">テンプレートファイルページ数</param>
        /// <param name="orientation">テンプレートファイル用紙方向</param>
        public void CreatePdf(string outputFilePath, string templateFilePath, int templateTargetPage, PaperOrientation orientation)
        {
            this.Orientation = orientation;

            // todo: すでにオープンされている場合の動作（テンプレート、本体）　未定

            // todo nullチェックは？
            // テンプレートオープン
            if (!File.Exists(templateFilePath))
                throw new FileNotFoundException(templateFilePath);
            var templatePdfReader = new PdfReader(templateFilePath);
            if (templateTargetPage > templatePdfReader.NumberOfPages)
                throw new ArgumentOutOfRangeException(nameof(templateTargetPage));
            


            this.TargetPdf = new Document();// サイズ指定は必要？
            this.TargetPdfWriter = PdfWriter.GetInstance(TargetPdf, new FileStream(outputFilePath, FileMode.Create));
            this.TargetPdf.Open();

            this.TargetPdfWriter.GetImportedPage(templatePdfReader, templateTargetPage);
        }

        public void AddPage()
        {
            this.CurrentPage++;
        }
    }
}
