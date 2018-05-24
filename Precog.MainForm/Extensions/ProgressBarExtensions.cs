using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Precog.MainForm.Extensions
{
    public enum ProgressBarColor
    {
        Green = 1,
        Red = 2,
        Orange = 3
    }

    public static class ProgressBarExtensions
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, ProgressBarColor state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}
