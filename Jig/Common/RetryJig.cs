using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jig.Common
{
    /// <summary>
    /// リトライ装置
    /// </summary>
    public class RetryJig
    {
        /// <summary>
        /// メソッドのリトライ実行を行います
        /// </summary>
        /// <param name="someMethod">リトライ実行を行うメソッド</param>
        /// <param name="retryCounts">リトライ回数</param>
        /// <param name="retryWaitMiliSeconds">リトライ待機秒数</param>
        public static void Retry(Action someMethod, int retryCounts, int retryWaitMiliSeconds)
        {
            Exception exStack = null;

            foreach (var i in Enumerable.Range(1, retryCounts + 1))
            {
                try
                {
                    someMethod();
                    return;
                }
                catch (Exception ex)
                {
                    exStack = exStack ?? ex;
                }

                Thread.Sleep(retryWaitMiliSeconds);
            }

            ExceptionDispatchInfo.Capture(exStack).Throw();
        }

        /// <summary>
        /// メソッドのリトライ実行を行います
        /// </summary>
        /// <param name="someMethod">リトライ実行を行うメソッド</param>
        /// <param name="retryCounts">リトライ回数</param>
        /// <param name="retryWaitMiliSeconds">リトライ待機秒数</param>
        public static void Retry(Action someMethod, int retryCounts, int retryWaitMiliSeconds, Action<Exception> onError)
        {
            Exception exStack = null;

            foreach (var i in Enumerable.Range(1, retryCounts + 1))
            {
                try
                {
                    someMethod();
                    return;
                }
                catch (Exception ex)
                {
                    exStack = exStack ?? ex;

                    onError(ex);
                }

                Thread.Sleep(retryWaitMiliSeconds);
            }

            ExceptionDispatchInfo.Capture(exStack).Throw();
        }
    }
}
