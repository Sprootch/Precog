using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Precog.IntegrationTests
{
    [TestClass]
    public class PrecogApplicationTests
    {
        private PrecogUI UI;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            UI = new PrecogUI();

            UI.StartApplication();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            UI.StopApplication();
        }

        [TestMethod]
        [DeploymentItem("TestData\\Empty.config")]
        public void CanParseOneConfigFile()
        {
            UI.DirectoryTextBox.Text = TestContext.TestDeploymentDir;

            UI.StartButton.Click();

            var outputBox = UI.OutputTextBox;
            StringAssert.Contains(outputBox.Text, "Empty.config");
            StringAssert.Contains(outputBox.Text, "OK");

            Assert.AreEqual(100, UI.ProgressBar.Value);
        }

        [TestMethod]
        [DeploymentItem("TestData\\Invalid.config")]
        public void CanDetectInvalidConfig()
        {
            UI.DirectoryTextBox.Text = TestContext.TestDeploymentDir;

            UI.StartButton.Click();

            var outputBox = UI.OutputTextBox;
            StringAssert.Contains(outputBox.Text, "Invalid.config");
            StringAssert.Contains(outputBox.Text, "ERROR");
        }

        [TestMethod]
        [DeploymentItem("TestData\\BindingMismatch.config")]
        public void CanDetectBindingMismatch()
        {
            UI.DirectoryTextBox.Text = TestContext.TestDeploymentDir;

            UI.StartButton.Click();

            var outputBox = UI.OutputTextBox;
            StringAssert.Contains(outputBox.Text, "BindingMismatch.config");
            StringAssert.Contains(outputBox.Text, "ERROR");
            StringAssert.Contains(outputBox.Text, "NOT_MATCHED");
        }
    }
}
