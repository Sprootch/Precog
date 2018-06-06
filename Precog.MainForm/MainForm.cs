using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Precog.Core;
using Precog.MainForm.Extensions;

namespace Precog.MainForm
{
    public partial class MainForm : Form
    {
        private readonly ConfigFileChecker _checker;

        public MainForm()
        {
            InitializeComponent();

            pathTextBox.Text = string.IsNullOrWhiteSpace(Properties.Settings.Default.DefaultDirectoryUserSetting)
                ? Properties.Settings.Default.DefaultDirectoryAppSetting
                : Properties.Settings.Default.DefaultDirectoryUserSetting;

            _checker = new ConfigFileChecker();
            _checker.AddExcludes("NLog.config", "transform", "artifact_bkp", "artifact\\default", "OLD_CONFIG");
            _checker.ProgressChanged += _checker_ProgressChanged;
        }

        private void _checker_ProgressChanged(object sender, int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => _checker_ProgressChanged(sender, value)));
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SaveUserSettings();

            button1.Enabled = false;
            outputTextBox.ResetText();
            progressBar1.SetState(ProgressBarColor.Green);
            progressBar1.Value = 0;

            try
            {
                await CheckConfigFiles();

                progressBar1.Value = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Precog", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Value = 100;
                button1.Enabled = true;
            }
        }

        private void SaveUserSettings()
        {
            Properties.Settings.Default.DefaultDirectoryUserSetting = pathTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private async Task CheckConfigFiles()
        {
            await _checker.CheckAsync(pathTextBox.Text, new ProgressDisplayer(this));
        }

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private class ProgressDisplayer : IProgress<ConfigMessage>
        {
            ProgressBarColor _currentColor;

            public ProgressDisplayer(MainForm form)
            {
                Form = form;
                _currentColor = ProgressBarColor.Green;
            }

            public MainForm Form { get; }

            public void Report(ConfigMessage message)
            {
                if (Form.outputTextBox.InvokeRequired)
                {
                    Form.outputTextBox.Invoke(new MethodInvoker(() => Report(message)));
                }
                else
                {
                    UpdateDisplay(message);
                }
            }

            private void UpdateDisplay(ConfigMessage message)
            {
                if (message.Severity == Severity.Success)
                {
                    Form.outputTextBox.AppendLine(message.Message, Color.Green);
                }
                else if (message.Severity == Severity.Warning)
                {
                    if (_currentColor == ProgressBarColor.Green) _currentColor = ProgressBarColor.Orange;

                    Form.outputTextBox.AppendLine(message.Message, Color.Orange);
                    Form.progressBar1.SetState(_currentColor);
                    Form.progressBar1.Invalidate();
                    Form.progressBar1.Update();
                    Form.progressBar1.Refresh();
                }
                else if (message.Severity == Severity.Error)
                {
                    _currentColor = ProgressBarColor.Red;

                    Form.outputTextBox.AppendLine(message.Message, Color.Red);
                    Form.progressBar1.SetState(_currentColor);
                    Form.progressBar1.Invalidate();
                    Form.progressBar1.Update();
                    Form.progressBar1.Refresh();
                }
                else
                {
                    Form.outputTextBox.AppendLine(message.Message);
                }

                Form.outputTextBox.SelectionStart = Form.outputTextBox.Text.Length;
                Form.outputTextBox.ScrollToCaret();
            }
        }
    }
}
