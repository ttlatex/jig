using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JigTest
{
    public class Class1
    {
        private int ggg;

        [Test]
        public void testm()
        {
            Console.WriteLine(nameof(ggg));
        }
    }
}
