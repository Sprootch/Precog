﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Precog.Core;

namespace Precog.MainForm
{
    public partial class Form1 : Form
    {
        public Form1()
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

            progressBar1.Update(100, error ? Brushes.Red : Brushes.Green);
        }
    }
}
