using CommandLine;
using Jig.Cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1.Setting
{
    public class CmdOptions : ICmdOptions
    {
        [Option('t', MetaValue = "タイトル", Required = true)]
        public string TitleName { get; set; }

        [Option('s', MetaValue = "サブタイトル")]
        public string SubTitleName { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }
    }
}
