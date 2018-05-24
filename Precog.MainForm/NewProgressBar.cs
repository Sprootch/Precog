using System.Drawing;
using System.Windows.Forms;

namespace Precog.MainForm
{
    public class NewProgressBar : ProgressBar
    {
        Brush brush = Brushes.Green;

        public NewProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        public void Update(int value, Brush color)
        {
            this.Value = value;
            brush = color;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 4;
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
        }
    }
}
