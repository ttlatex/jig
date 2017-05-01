using B1.Value;
using Jig.Pdf;
using Jig.Pdf.PdfConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.Business
{
    public class PdfCreator
    {
        /// <summary>
        /// 出力ファイルパス
        /// </summary>
        private string outputPath;
        /// <summary>
        /// テンプレートパス
        /// </summary>
        private string templatePath;

        public PdfCreator(string templatePath, string outputPath)
        {
            this.templatePath = templatePath;
            this.outputPath = outputPath;
        }

        public void OutputPDF(List<AssortPdfValue> parameters)
        {
            var direction = PaperOrientation.Vertical;

            using (var pdfEditor = new PdfEditorLight())
            {
                // 新規空PDF作成
                pdfEditor.CreatePdf(this.outputPath, this.templatePath, 1, direction);

                foreach (var pageValue in parameters)
                {
                    pdfEditor.AddPage();
                    this.EmbeddedString(pdfEditor, pageValue);
                }
            }
        }

        private void EmbeddedString(PdfEditorLight pdfEditor, AssortPdfValue pageValue)
        {
            // 共通部貼り付け
            pdfEditor.SetText(target: pageValue.Title,
                x: 250f, y: 70f, fontSize: 12f, fontName: FontName.Gothic, align: Align.Center);
            pdfEditor.SetText(target: pageValue.Page,
                x: 250f, y: 400f, fontSize: 7f, fontName: FontName.Gothic, align: Align.Center);

            // 明細部貼り付け
            float interval = 17.55f;
            pageValue.Detail.ForEach((x, i) =>
            {
                pdfEditor.SetText(target: x.No,
                    x: 30f, y: 100f + interval * i, fontSize: 8f, fontName: FontName.Gothic, align: Align.Center);
                pdfEditor.SetText(target: x.Name,
                    x: 80f, y: 100f + interval * i, fontSize: 8f, fontName: FontName.Gothic, align: Align.Center);
            });
        }
    }
}
