using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Settings
{
    /// <summary>
    /// コマンドライン引数格納用クラスが利用します
    /// </summary>
    public interface ICmdOptions
    {
        /// <summary>
        /// コマンドライン引数格納用クラスが利用します
        /// </summary>
        [ParserState]
        IParserState LastParserState { get; set; }
    }
}
