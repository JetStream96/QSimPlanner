using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Controllers
{
    public class FadeoutController
    {
        private Control control;

        public FadeoutController(Control control)
        {
            this.control = control;
        }

        public void Fadeout()
        {
            control.ForeColor = Color.Black;
            var timer = new Timer();
            timer.Interval = 20;
            timer.Tick += (s, e) =>
            {
                if (control.ForeColor.R == 255)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                else
                {
                    int r = Math.Min(control.ForeColor.R + 20, 255);
                    control.ForeColor = Color.FromArgb(r, r, r);
                }
            };

            timer.Start();
        }
    }
}
