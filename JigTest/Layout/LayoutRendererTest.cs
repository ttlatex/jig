using Jig.Layout;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JigTest.Layout
{
    public class LayoutRendererTest
    {
        [Test]
        public void ShortDate()
        {
            // set
            string penguin = "${shortdate}/mypenguin".Render();
            string expect = DateTime.Now.ToString("yyyyMMdd") + "/mypenguin";

            // assart
            Assert.That(penguin, Is.EqualTo(expect));
        }

        [Test]
        public void CustomDate()
        {
            // set
            string penguin = "${customdate:HH時}/mypenguin_${customdate:yyyy年MM月}".Render();
            string expect = DateTime.Now.ToString("HH時") + "/mypenguin_" + DateTime.Now.ToString("yyyy年MM月");

            // assart
            Assert.That(penguin, Is.EqualTo(expect));
        }

        [Test]
        public void CompositeT()
        {
            // set
            string penguin = "${shortdate}/${machinename}${username}_${customdate:MM月}".Render();
            string expect = DateTime.Now.ToString("yyyyMMdd") + "/" + Environment.MachineName + Environment.UserName + "_" + DateTime.Now.ToString("MM月");

            // assert
            Assert.That(penguin, Is.EqualTo(expect));
        }
    }
}
