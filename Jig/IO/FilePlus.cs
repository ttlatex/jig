using Jig.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.IO
{
    /// <summary>
    /// ファイル操作を提供します
    /// </summary>
    public class FilePlus
    {
        /// <summary>
        /// ファイル操作リトライ回数
        /// </summary>
        private int FileRetryCount = 3;
        /// <summary>
        /// ファイル操作リトライ待機秒数
        /// </summary>
        private int FileRetryWaitSecond = 2;
        /// <summary>
        /// コンフィグセクションを読み込んだか
        /// </summary>
        public bool IsReadSection { private set; get; }

        #region 生成部
        /// <summary>
        /// 何も指定しなかった場合コンフィグ内"FilePlusSettings"セクションが読み込まれます
        /// </summary>
        public FilePlus()
        : this("FilePlusSettings")
        {
        }

        /// <summary>
        /// コンフィグセクションを指定して読み込み
        /// </summary>
        /// <param name="sectionName">セクション名 NameValueCollection</param>
        public FilePlus(string sectionName)
        {
            // コンフィグセクション読み込み
            this.IsReadSection = false;
            var settings = ConfigurationManager.GetSection(sectionName) as NameValueCollection;  // NameValueConfigurationCollectionは？
            if (settings == null) return;
            this.IsReadSection = true;

            // リトライカウント設定
            string inFileRetryCount = settings[nameof(FileRetryCount)];
            if (!string.IsNullOrEmpty(inFileRetryCount))
            {
                if (!int.TryParse(inFileRetryCount, out this.FileRetryCount))
                    throw new ArgumentException(nameof(FileRetryCount) + "が整数値でありません");
                if (this.FileRetryCount < 0)
                    throw new ArgumentException(nameof(FileRetryCount) + "は0以上を設定してください");
            }

            // リトライ秒数設定
            string inFileRetryWaitSecond = settings[nameof(FileRetryWaitSecond)];
            if (!string.IsNullOrEmpty(inFileRetryWaitSecond))
            {
                if (!int.TryParse(inFileRetryWaitSecond, out this.FileRetryWaitSecond))
                    throw new ArgumentException(nameof(FileRetryWaitSecond) + "が整数値でありません");
                if (this.FileRetryWaitSecond < 0)
                    throw new ArgumentException(nameof(FileRetryWaitSecond) + "は0以上を設定してください");
            }
        }

        #endregion

        #region ファイル操作
        /// <summary>
        /// リトライファイルコピー
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public void RetryCopy(string sourceFileName, string destFileName)
        {
            FileRetry(() => File.Copy(sourceFileName, destFileName));
        }

        /// <summary>
        /// リトライファイル移動
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public void RetryMove(string sourceFileName, string destFileName)
        {
            FileRetry(() => File.Move(sourceFileName, destFileName));
        }

        /// <summary>
        /// リトライファイルデリート
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public void RetryDelete(string sourceFileName, string destFileName)
        {
            FileRetry(() => File.Delete(sourceFileName));
        }

        /// <summary>
        /// リトライディレクトリ削除
        /// サブディレクトリ、サブフォルダも削除
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public void RetryDeleteDirectory(string sourceFileName, string destFileName)
        {
            FileRetry(() => Directory.Delete(sourceFileName, true));
        }
        #endregion

        /// <summary>
        /// リトライ装置のラッパー
        /// </summary>
        /// <param name="method"></param>
        private void FileRetry(Action method)
        {
            RetryJig.Retry(method, this.FileRetryCount, this.FileRetryWaitSecond);
        }

        /// <summary>
        /// メンバ変数表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"[{nameof(FileRetryCount)}]{this.FileRetryCount}\r\n" +
                $"[{nameof(FileRetryWaitSecond)}]{this.FileRetryWaitSecond}\r\n";
        }
    }
}
