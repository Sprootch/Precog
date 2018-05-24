using System;
using System.Drawing;
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

        private void button1_Click(object sender, EventArgs e)
        {
            outputTextBox.ResetText();
            progressBar1.Value = 0;

            var checker = new ConfigFileChecker();
            checker.AddExcludes("NLog.config", "transform", "artifact_bkp", "artifact\\default");

            var results = checker.Check(pathTextBox.Text);

            bool error = false;
            foreach (var result in results)
            {
                outputTextBox.AppendLine(result.ConfigFile);

                if (result.Status == ConfigStatus.Success)
                {
                    outputTextBox.AppendText(result.Result, Color.Green);
                }
                else
                {
                    outputTextBox.AppendText(result.Result, Color.Red);
                    error = true;
                }
            }

            progressBar1.Value = 100;
            progressBar1.SetState(error ? ProgressBarColor.Red : ProgressBarColor.Green);
            progressBar1.Invalidate();
            progressBar1.Refresh();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
