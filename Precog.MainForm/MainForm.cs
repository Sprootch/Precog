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
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            outputTextBox.ResetText();
            progressBar1.Value = 0;

            bool error = false;
            try
            {
                await CheckConfigFiles();

                progressBar1.Value = 100;
                progressBar1.SetState(error ? ProgressBarColor.Red : ProgressBarColor.Green);
                progressBar1.Invalidate();
                progressBar1.Refresh();
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

        private async Task CheckConfigFiles()
        {
            var checker = new ConfigFileChecker();
            checker.AddExcludes("NLog.config", "transform", "artifact_bkp", "artifact\\default", "OLD_CONFIG");

            await checker.CheckAsync(pathTextBox.Text, new ProgressDisplayer(this));

            //bool error = false;
            //foreach (var result in results)
            //{
            //    outputTextBox.AppendLine(result.ConfigFile);

            //    if (result.Status == Severity.Success)
            //    {
            //        outputTextBox.AppendText(result.Result, Color.Green);
            //    }
            //    else
            //    {
            //        outputTextBox.AppendText(result.Result, Color.Red);
            //        error = true;
            //    }
            //}

            //return error;
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
                if (message.Severity == Severity.Success)
                {
                    Form.outputTextBox.AppendLine(message.Message, Color.Green);
                }
                else if (message.Severity == Severity.Error)
                {
                    Form.outputTextBox.AppendLine(message.Message, Color.Red);
                }
                else
                {
                    Form.outputTextBox.AppendLine(message.Message);
                }
            }
        }
    }
}
