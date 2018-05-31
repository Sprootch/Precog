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
        public MainForm()
        {
            InitializeComponent();

            pathTextBox.Text = string.IsNullOrWhiteSpace(Properties.Settings.Default.DefaultDirectoryUserSetting) 
                ? Properties.Settings.Default.DefaultDirectoryAppSetting
                : Properties.Settings.Default.DefaultDirectoryUserSetting;
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
            var checker = new ConfigFileChecker();
            checker.AddExcludes("NLog.config", "transform", "artifact_bkp", "artifact\\default", "OLD_CONFIG");

            await checker.CheckAsync(pathTextBox.Text, new ProgressDisplayer(this));
        }

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private class ProgressDisplayer : IProgress<ConfigMessage>
        {
            public ProgressDisplayer(MainForm form)
            {
                Form = form;
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
                    if (message.Severity == Severity.Success)
                    {
                        Form.outputTextBox.AppendLine(message.Message, Color.Green);
                    }
                    else if (message.Severity == Severity.Error)
                    {
                        Form.outputTextBox.AppendLine(message.Message, Color.Red);
                        Form.progressBar1.Value = 100;
                        Form.progressBar1.SetState(ProgressBarColor.Red);
                        Form.progressBar1.Invalidate();
                        Form.progressBar1.Update();
                        Form.progressBar1.Refresh();
                    }
                    else
                    {
                        Form.outputTextBox.AppendLine(message.Message);
                    }
                }
            }
        }
    }
}
