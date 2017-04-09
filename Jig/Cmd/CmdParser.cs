using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Cmd
{
    // nugetでCommand Line Parserをダウンロードしてください
    // 参考サイト
    // https://wiki.dobon.net/index.php?.NET%A5%D7%A5%ED%A5%B0%A5%E9%A5%DF%A5%F3%A5%B0%B8%A6%B5%E6%2F108
    // https://wiki.dobon.net/index.php?.NET%A5%D7%A5%ED%A5%B0%A5%E9%A5%DF%A5%F3%A5%B0%B8%A6%B5%E6%2F109
    // https://wiki.dobon.net/index.php?.NET%A5%D7%A5%ED%A5%B0%A5%E9%A5%DF%A5%F3%A5%B0%B8%A6%B5%E6%2F110

    public static class CmdParser
    {
        /// <summary>
        /// Optionsクラスのインスタンスを作成します
        /// </summary>
        public static Options OptionsInstance<Options>(string[] args) where Options : ICmdOptions, new()
        {
            Options cmds = new Options();

            var isSuccess = CommandLine.Parser.Default.ParseArguments(args, cmds);

            if (!isSuccess)
            {
                var msg = cmds.GetUsage();
                Console.WriteLine(msg);
                throw new ArgumentException(msg);
            }

            return cmds;
        }

        /// <summary>
        /// 解析失敗時のメッセージ
        /// </summary>
        public static string GetUsage(this ICmdOptions options)
        {
            var help = new HelpText();
            help.AddDashesToOption = true;

            if (options.LastParserState == null)
            {
                // --help
                help.AddPreOptionsLine("使用できるオプション一覧：");
            }
            else if (options.LastParserState.Errors != null && options.LastParserState.Errors.Count > 0)
            {
                // 解析エラー
                var errMsg = help.RenderParsingErrorsText(options, 0);
                help.AddPreOptionsLine(errMsg);
            }
            else
            {
                // 不明なオプション
                help.AddPreOptionsLine("想定していないオプションが含まれています");
                help.AddPreOptionsLine("以下に使用できるオプションの一覧を表示します");
            }
            help.AddOptions(options);
            help.AddPostOptionsLine("");

            return help.ToString();
        }
    }
}
