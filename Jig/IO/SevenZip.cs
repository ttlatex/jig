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
        #region メンバ変数
        /// <summary>
        /// 7z実行ファイルパス
        /// </summary>
        private string SevenZipExePath;
        /// <summary>
        /// リトライ回数
        /// </summary>
        private int RetryCount;
        /// <summary>
        /// リトライ秒数
        /// </summary>
        private int RetryWaitSecond;
        /// <summary>
        /// プロセスタイムアウト秒数
        /// </summary>
        private int ProcessTimeoutSecond = 60;

        /// <summary>
        /// 初期化の際にコンフィグセクションを読み込んだかどうか
        /// </summary>
        public bool IsReadSection { private set; get; }
        #endregion

        /// <summary>
        /// 生成部 "SevenZipSettings"セクションを読み込みます
        /// </summary>
        public SevenZip()
            : this("SevenZipSettings")
        {
        }

        /// <summary>
        /// コンフィグセクション指定読み込み
        /// </summary>
        /// <param name="sectionName"></param>
        public SevenZip(string sectionName)
        {
            // コンフィグセクション読込
            var settings = ConfigurationManager.GetSection(sectionName) as NameValueCollection; // NamevalueConfigCollectionでは？
            if (settings == null) throw new Exception(sectionName + "セクションの読み込みに失敗しました");
            this.IsReadSection = true;

            // 7z実行パス設定
            this.SevenZipExePath = settings["SevenZipExePath"];
            AssertConfig.IsNull(() => this.SevenZipExePath);
            AssertConfig.IsNotExistFile(() => this.SevenZipExePath);


            // リトライ回数
            var _RetryCount = settings["RetryCount"];
            if (!string.IsNullOrEmpty(_RetryCount))
            {
                if (!int.TryParse(_RetryCount, out this.RetryCount))
                    throw new ArgumentException("整数値を設定してください 項目名:" + nameof(this.RetryCount));
                if (this.RetryCount < 0)
                    throw new ArgumentException("0以上を設定してください 項目名:" + nameof(this.RetryCount));
            }

            // リトライ秒数
            var _RetryWaitSecond = settings["RetryWaitSecond"];
            if (!string.IsNullOrEmpty(_RetryCount))
            {
                if (!int.TryParse(_RetryWaitSecond, out this.RetryWaitSecond))
                    throw new ArgumentException("整数値を設定してください 項目名:" + nameof(this.RetryWaitSecond));
                if (this.RetryCount < 0)
                    throw new ArgumentException("0以上を設定してください 項目名:" + nameof(this.RetryWaitSecond));
            }

            // プロセスタイムアウト秒数秒数
            var _ProcessTimeoutSecond = settings["ProcessTimeoutSecond"];
            if (!string.IsNullOrEmpty(_RetryCount))
            {
                if (!int.TryParse(_ProcessTimeoutSecond, out this.ProcessTimeoutSecond))
                    throw new ArgumentException("整数値を設定してください 項目名:" + nameof(this.ProcessTimeoutSecond));
                if (this.RetryCount < 0)
                    throw new ArgumentException("0以上を設定してください 項目名:" + nameof(this.ProcessTimeoutSecond));
            }
        }

        #region 暗号化

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

        #endregion

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

                if (this.ProcessTimeoutSecond == 0)
                    // 無限待機注意
                    process.WaitForExit();
                else
                {
                    var isFinished = process.WaitForExit(this.ProcessTimeoutSecond * 1000);
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
                if (!process.HasExited)
                    process.Dispose();
            }
        }

        /// <summary>
        /// リトライ装置のラッパー
        /// </summary>
        /// <param name="someMethod"></param>
        private void ZipRetry(Action someMethod)
        {
            RetryJig.Retry(someMethod, this.RetryCount, this.RetryWaitSecond);
        }
    }
}
