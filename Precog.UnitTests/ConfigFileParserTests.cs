using Microsoft.VisualStudio.TestTools.UnitTesting;
using Precog.Core;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Precog.UnitTests
{
    [TestClass]
    public class ConfigFileParserTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckIfConfigFilePathIsNull()
        {
            var parser = new ConfigFileParser(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckIfConfigExists()
        {
            var nonExistingConfigFile = "blaba.config";
            var parser = new ConfigFileParser(nonExistingConfigFile);
        }

        [TestMethod]
        [DeploymentItem("TestData\\NoService.config")]
        public void CanParseEvenIfNoService()
        {
            var parser = new ConfigFileParser(Path.Combine(TestContext.DeploymentDirectory, "NoService.config"));

            var output = parser.GetServices();

            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
        [DeploymentItem("TestData\\TestConfig.config")]
        public void CanParseMultipleServices()
        {
            var parser = new ConfigFileParser(Path.Combine(TestContext.DeploymentDirectory, "TestConfig.config"));

            var output = parser.GetServices();

            var svc1 = output.First();
            Assert.AreEqual("net.tcp://localhost/CensyActionListProvider/Service.svc", svc1.Address);
            Assert.AreEqual("SYS_CENSY_DEV@stib-mivb.be", svc1.Identity);

            var svc2 = output.Last();
            Assert.AreEqual("net.tcp://dsal-apbil02:7990/v1.0/Interop_GreenlistProvider", svc2.Address);
            Assert.AreEqual("SYS_SALES_DEV@stib-mivb.be", svc2.Identity);
        }

        [TestMethod]
        [DeploymentItem("TestData\\BindingMismatch.config")]
        [ExpectedException(typeof(ConfigurationErrorsException))] 
        public void CanDetectBindingMismatch()
        {
            var parser = new ConfigFileParser(Path.Combine(TestContext.DeploymentDirectory, "BindingMismatch.config"));

            var output = parser.GetServices();
        }
    }
}
