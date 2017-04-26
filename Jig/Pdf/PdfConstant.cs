using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Pdf.PdfConstant
{
    /// <summary>
    /// 合わせ
    /// </summary>
    public enum Align
    {
        Left,
        Center,
        Right,
    }

    /// <summary>
    /// 用紙方向
    /// </summary>
    public enum PaperOrientation
    {
        /// <summary>
        /// 垂直方向
        /// </summary>
        Vertical,
        /// <summary>
        /// 水平方向
        /// </summary>
        Horizontal,
    }

    /// <summary>
    /// フォント
    /// </summary>
    public enum FontName
    {
        OCRB,
        Gothic,
        PGothic,
        Mincho,
        PMincho,
    }
}
