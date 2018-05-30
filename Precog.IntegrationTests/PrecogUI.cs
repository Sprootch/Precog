using System;
using System.IO;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Precog.IntegrationTests
{
    internal sealed class PrecogUI
    {
        private Application _precogApp;
        private Lazy<Window> _mainWindow;
        private Lazy<TextBox> _dirTextBox;
        private Lazy<TextBox> _outputTextBox;
        private Lazy<Button> _startButton;
        private Lazy<ProgressBar> _progressBar;

        public Window MainWindow => _mainWindow.Value;
        public TextBox DirectoryTextBox => _dirTextBox.Value;
        public TextBox OutputTextBox => _outputTextBox.Value;
        public Button StartButton => _startButton.Value;
        public ProgressBar ProgressBar => _progressBar.Value;

        //public static PrecogUI Instance { get; } = new PrecogUI();

        public PrecogUI()
        {
            _mainWindow = new Lazy<Window>(() => _precogApp.GetWindow(SearchCriteria.ByText("Precog"), InitializeOption.NoCache));
            _dirTextBox = new Lazy<TextBox>(() => MainWindow.Get<TextBox>("pathTextBox"));
            _outputTextBox = new Lazy<TextBox>(() => MainWindow.Get<TextBox>("outputTextBox"));
            _startButton = new Lazy<Button>(() => MainWindow.Get<Button>(SearchCriteria.ByText("Start")));
            _progressBar = new Lazy<ProgressBar>(() => MainWindow.Get<ProgressBar>("progressBar1"));
        }

        public void StartApplication()
        {
            var pathToExe = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, @"Precog.MainForm\bin\Debug\Precog.MainForm.exe");
            _precogApp = Application.Launch(pathToExe);
        }

        public void StopApplication()
        {
            MainWindow?.Close();
        }
    }
}
