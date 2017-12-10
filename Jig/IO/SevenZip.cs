using Jig.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.IO
{
    // 7zのコマンドライン引数は下記サイトを参考に
    // https://sevenzip.osdn.jp/chm/cmdline/

    public class SevenZip
    {
        /// <summary>
        /// 7z実行ファイルパス
        /// </summary>
        private string SevenZipExePath;
        /// <summary>
        /// リトライ回数
        /// </summary>
        private int RetryCounts;
        /// <summary>
        /// リトライ秒数
        /// </summary>
        private int RetryWaitMiliSeconds;
        /// <summary>
        /// プロセスタイムアウト秒数
        /// </summary>
        private int ProcessTimeoutMiliSeconds;

        private SevenZip() { }

        /// <summary>
        /// コンフィグファイルよりインスタンスを生成する
        /// </summary>
        /// <param name="sectionName">コンフィグセクション名</param>
        /// <returns>SevenZip</returns>
        public static SevenZip GetInstance(string sectionName = "SevenZipSettings")
        {
            // コンフィグセクション読込
            var settings = (SevenZipSettings)ConfigurationManager.GetSection(sectionName);
            if (settings == null)
                throw new Exception(sectionName + "セクションの読み込みに失敗しました");

            return new SevenZip
            {
                SevenZipExePath = settings.SevenZipExePath,
                RetryCounts = settings.RetryCounts,
                RetryWaitMiliSeconds = settings.RetryWaitMiliSeconds,
                ProcessTimeoutMiliSeconds = settings.ProcessTimeoutMiliSeconds,
            };
        }

        /// <summary>
        /// 7z暗号化
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="password"></param>
        /// <param name="compressLevel"></param>
        public void Encrypt7z(string sourceFileName, string destFileName, string password, int compressLevel)
        {
            CheckEncryptParam(sourceFileName, destFileName);

            var _password = (string.IsNullOrEmpty(password))
                ? ""
                : "-p" + password;

            string command = $"as -t7z -mx={compressLevel} {_password} \"{sourceFileName}\" \"{destFileName}\"";

            this.ZipRetry(() => this.Execute(command));
        }

        /// <summary>
        /// 暗号化引数チェク
        /// </summary>
        private void CheckEncryptParam(string sourceFileName, string destFileName)
        {
            if (string.IsNullOrEmpty(sourceFileName))
                throw new ArgumentException(nameof(sourceFileName));
            if (string.IsNullOrEmpty(destFileName))
                throw new ArgumentException(nameof(destFileName));
            if (!File.Exists(sourceFileName))
                throw new FileNotFoundException(sourceFileName);
            if (!Directory.Exists(Path.GetDirectoryName(destFileName)))
                throw new DirectoryNotFoundException(Path.GetDirectoryName(destFileName));
        }

        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="command"></param>
        private void Execute(string command)
        {
            var process = new Process();

            try
            {
                process.StartInfo.FileName = this.SevenZipExePath;
                process.StartInfo.Arguments = command;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;

                process.Start();

                if (this.ProcessTimeoutMiliSeconds == 0)
                    // 無限待機注意
                    process.WaitForExit();
                else
                {
                    var isFinished = process.WaitForExit(this.RetryWaitMiliSeconds * 1000);
                    if (isFinished)
                        throw new TimeoutException("7z.exeプロセスがタイムアウトしました");
                }

                if (process.ExitCode != 0)
                {
                    var message = $"7z実行時エラー ExitCode:{process.ExitCode}\n{process.StandardError.ReadToEnd()}";
                    throw new InvalidOperationException(message);
                }
            }
            finally
            {
                process.Dispose();
            }
        }

        /// <summary>
        /// リトライ装置のラッパー
        /// </summary>
        /// <param name="someMethod"></param>
        private void ZipRetry(Action someMethod)
        {
            RetryJig.Retry(someMethod, this.RetryCounts, this.RetryWaitMiliSeconds);
        }
    }
}
