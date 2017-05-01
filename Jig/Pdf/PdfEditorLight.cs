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
    public class PdfEditorLight : IDisposable
    {
        /// <summary>
        /// 用紙方向
        /// このプロパティ必要？
        /// </summary>
        private PaperOrientation orientation;
        /// <summary>
        /// 出力PDF
        /// </summary>
        private Document targetPdf;
        /// <summary>
        /// PDFライター
        /// </summary>
        private PdfWriter targetPdfWriter;
        /// <summary>
        /// テンプレート
        /// </summary>
        private PdfImportedPage templatePage;

        private Dictionary<FontName, BaseFont> fonts;


        #region 生成
        public PdfEditorLight()
        {
            const string msgothicPath = @"C:\Windows\Fonts\msgothic.ttc";
            const string msminchoPath = @"C:\Windows\Fonts\msmincho.ttc";
            const string ocrbPath = @"C:\Windows\Fonts\OCRB.TTF";

            if (!File.Exists(msgothicPath))
                throw new FileNotFoundException(msgothicPath);
            if (!File.Exists(msminchoPath))
                throw new FileNotFoundException(msminchoPath);
            if (!File.Exists(ocrbPath))
                throw new FileNotFoundException(ocrbPath);

            // フォントの事前読み込み
            var gothic = BaseFont.CreateFont(msgothicPath + ",0", BaseFont.IDENTITY_H, true);
            var pgothic = BaseFont.CreateFont(msgothicPath + ",1", BaseFont.IDENTITY_H, true);
            var mincho = BaseFont.CreateFont(msminchoPath + ",0", BaseFont.IDENTITY_H, true);
            var pmincho = BaseFont.CreateFont(msminchoPath + ",1", BaseFont.IDENTITY_H, true);
            var ocrb = BaseFont.CreateFont(ocrbPath, BaseFont.IDENTITY_H, true);

            fonts = new Dictionary<FontName, BaseFont>();
            fonts.Add(FontName.Gothic, gothic);
            fonts.Add(FontName.PGothic, pgothic);
            fonts.Add(FontName.Mincho, mincho);
            fonts.Add(FontName.PMincho, pmincho);
            fonts.Add(FontName.OCRB, ocrb);
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
            this.orientation = orientation;

            // todo: すでにオープンされている場合の動作（テンプレート、本体）　未定

            // todo nullチェックは？
            // テンプレートオープン
            if (!File.Exists(templateFilePath))
                throw new FileNotFoundException(templateFilePath);
            var templatePdfReader = new PdfReader(templateFilePath);
            if (templateTargetPage > templatePdfReader.NumberOfPages)
                throw new ArgumentOutOfRangeException(nameof(templateTargetPage));

            this.targetPdf = new Document();// サイズ指定は必要？
            this.targetPdfWriter = PdfWriter.GetInstance(targetPdf, new FileStream(outputFilePath, FileMode.Create));
            this.targetPdf.Open();

            this.templatePage = this.targetPdfWriter.GetImportedPage(templatePdfReader, templateTargetPage);
        }

        /// <summary>
        /// ページ追加
        /// </summary>
        public void AddPage()
        {
            this.targetPdf.NewPage();

            if (this.orientation == PaperOrientation.Horizontal)
                this.targetPdfWriter.DirectContent.AddTemplate(this.templatePage, 0, -1f, 1f, 0, 0, this.templatePage.Height);
            else
                this.targetPdfWriter.DirectContent.AddTemplate(this.templatePage, 1f, 0, 0, 1f, 0, 0);
        }

        public void SetText(string target, float x, float y, float fontSize, FontName fontName, Align align)
        {
            var content = this.targetPdfWriter.DirectContent;

            content.BeginText();
            content.SetFontAndSize(this.fonts[fontName], fontSize);
            content.ShowTextAligned((int)align, target, x, (this.templatePage.Height - y), 0);
            content.EndText();
        }

        /// <summary>
        /// 罫線追加
        /// </summary>
        public void SetRule(float lineWidth, float x1, float y1, float x2, float y2)
        {
            var content = this.targetPdfWriter.DirectContent;

            content.SetLineWidth(lineWidth);
            content.MoveTo(x1, (this.templatePage.Height - y1));
            content.LineTo(x2, (this.templatePage.Height - y2));
            content.Stroke();
        }

        #region dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
        #endregion
    }
}
