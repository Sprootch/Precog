using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Precog.Core;

namespace Precog.IntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var configFileChecker = new ConfigFileChecker();
            //configFileChecker.AddIgnore("", "", "");

            var result = configFileChecker.Check();

            Assert.AreEqual(result.Success, result.Status);
            Assert.AreEqual(1, result.Messages.Count);
        }
    }
}
