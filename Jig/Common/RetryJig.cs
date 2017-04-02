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
        /// <param name="retryCount">リトライ回数</param>
        /// <param name="retryWaitCount">リトライ待機秒数</param>
        public static void Retry(Action someMethod, int retryCount, int retryWaitCount)
        {
            Exception exStack = null;

            foreach (var i in Enumerable.Range(1, retryCount + 1))
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

                Thread.Sleep(retryWaitCount * 1000);
            }

            ExceptionDispatchInfo.Capture(exStack).Throw();
        }
    }
}
