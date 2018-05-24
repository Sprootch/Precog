using System;
using System.Drawing;
using System.Windows.Forms;

namespace Precog.MainForm
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text + Environment.NewLine);
            box.SelectionColor = box.ForeColor;
        }

        public static void AppendLine(this RichTextBox box, string text)
        {
            box.AppendText(text + Environment.NewLine);
        }
    }

}
