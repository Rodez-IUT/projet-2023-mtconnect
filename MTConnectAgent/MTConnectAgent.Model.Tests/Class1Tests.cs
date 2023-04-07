using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTConnectAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTConnectAgent.Model.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        [TestMethod()]
        public void TestProbeTest()
        {
            var test = new Class1();

            Console.WriteLine(test.TestProbe());
        }
    }
}