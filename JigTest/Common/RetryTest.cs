using Jig.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JigTest.Common
{
    class RetryTest
    {
        public void SomeMethod()
        {
            throw new ArgumentException("hogehoge");
        }

        [Test]
        public void _RetryTest()
        {
            try
            {
                RetryJig.Retry(() => SomeMethod(), 1, 1);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
