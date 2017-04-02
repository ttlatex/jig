using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Layout
{
    /// <summary>
    /// レンダリング前の文字列を表します
    /// 例：img_${shortdate}.png
    /// </summary>
    public class ModelString
    {
        /// <summary>
        /// レンダリング前の文字列です
        /// </summary>
        public string PlainText { get; private set; }

        public ModelString(string plainText)
        {
            this.PlainText = plainText;
        }

        public string Render()
        {
            return LayoutRenderer.Render(this.PlainText);
        }

        public static implicit operator String(ModelString modelString)
        {
            return modelString.Render();
        }
    }
}
