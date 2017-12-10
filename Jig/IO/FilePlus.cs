using Jig.Common;
using System;
using System.Configuration;
using System.IO;

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
        private int FileRetryCounts;
        /// <summary>
        /// ファイル操作リトライ待機秒数(ms)
        /// </summary>
        private int FileRetryWaitMiliSeconds;

        /// <summary>
        /// コンフィグファイルよりインスタンスを生成する
        /// </summary>
        /// <param name="sectionName">コンフィグセクション名</param>
        /// <returns>FilePlus</returns>
        public FilePlus GetInstance(string sectionName = "FilePlusSettings")
        {
            var settings = (FilePlusSettings)ConfigurationManager.GetSection(sectionName);

            if (settings == null)
                return new FilePlus
                {
                    FileRetryCounts = 3,
                    FileRetryWaitMiliSeconds = 2000,
                };

            return new FilePlus
            {
                FileRetryCounts = settings.FileRetryCounts,
                FileRetryWaitMiliSeconds = settings.FileRetryWaitMiliSeconds,
            };
        }
        
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

        /// <summary>
        /// リトライ装置のラッパー
        /// </summary>
        /// <param name="method"></param>
        private void FileRetry(Action method)
        {
            RetryJig.Retry(method, this.FileRetryCounts, this.FileRetryWaitMiliSeconds);
        }
    }
}
