using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using conference1;
using static conference1.MainWindow;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var context = helper.GetContext();

            Assert.IsNotNull(context);
        }
    }
}
