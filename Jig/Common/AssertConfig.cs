using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Common
{
    // 記法を統一しているが特に意味はない

    public static class AssertConfig
    {
        /// <summary>
        /// nullなら例外
        /// </summary>
        /// <param name="e"></param>
        public static void IsNull(Expression<Func<string>> e)
        {
            var value = e.Compile()();
            var member = (MemberExpression)e.Body;

            if (value == null)
                throw new ArgumentNullException("null パラメータ:" + member);
        }

        /// <summary>
        /// 整数以外で例外
        /// </summary>
        /// <param name="e"></param>
        public static void IsNotNumeric(Expression<Func<string>> e)
        {
            var value = e.Compile()();
            var member = (MemberExpression)e.Body;

            if (value == null)
                throw new ArgumentNullException("数値でありません パラメータ:" + member);
        }

        /// <summary>
        /// ファイル存在無しで例外
        /// </summary>
        /// <param name="e"></param>
        public static void IsNotExistFile(Expression<Func<string>> e)
        {
            var value = e.Compile()();

            if (!File.Exists(value))
                throw new FileNotFoundException(value);
        }
    }
}
