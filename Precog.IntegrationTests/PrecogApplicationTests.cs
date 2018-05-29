using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Precog.IntegrationTests
{
    [TestClass]
    public class PrecogApplicationTests
    {
        Application _precogApp;
        Window _mainWindow;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            _precogApp = Application.Launch(@"C:\Users\Vincent Delcoigne\Source\Repos\Precog\Precog.MainForm\bin\Debug\Precog.MainForm.exe");

            _mainWindow = _precogApp.GetWindow(SearchCriteria.ByText("Precog"), InitializeOption.NoCache);
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            _mainWindow?.Close();
        }

        [TestMethod]
        [DeploymentItem("TestData\\Empty.config")]
        public void CanParseOneConfigFile()
        {
            var textBox = _mainWindow.Get<TextBox>("pathTextBox");
            textBox.Text = TestContext.TestDeploymentDir;

            var button = _mainWindow.Get<Button>(SearchCriteria.ByText("Start"));
            button.Click();

            var outputBox = _mainWindow.Get<TextBox>("outputTextBox");
            StringAssert.Contains(outputBox.Text, "Empty.config");
            StringAssert.Contains(outputBox.Text, "OK");
        }

        [TestMethod]
        [DeploymentItem("TestData\\Invalid.config")]
        public void CanDetectInvalidConfigFiles()
        {
            var textBox = _mainWindow.Get<TextBox>("pathTextBox");
            textBox.Text = TestContext.TestDeploymentDir;

            var button = _mainWindow.Get<Button>(SearchCriteria.ByText("Start"));
            button.Click();

            var outputBox = _mainWindow.Get<TextBox>("outputTextBox");
            StringAssert.Contains(outputBox.Text, "Invalid.config");
            StringAssert.Contains(outputBox.Text, "ERROR");
        }
    }
}
