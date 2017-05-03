using Jig.Layout;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JigTest.Layout
{
    public class ModelStringTest
    {
        public void explictString1()
        {
            // set
            ModelString mStr = new ModelString("penguin${shortdate}");
            var penguin = (string)mStr;

            string expect = "penguin" + DateTime.Now.ToString("yyyyMMdd");

            // assert
            Assert.That(penguin, Is.EqualTo(expect));
        }

        public void explictString2()
        {
            // set
            ModelString mStr = new ModelString("penguin${shortdate}");
            var penguin = mStr + "_A";

            string expect = "penguin" + DateTime.Now.ToString("yyyyMMdd") + "_A";

            // assert
            Assert.That(penguin, Is.EqualTo(expect));
        }
    }
}
