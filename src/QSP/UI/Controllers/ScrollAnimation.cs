using System;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Math;

namespace QSP.UI.Controllers
{
    public static class ScrollAnimation
    {
        public static void ScrollToPosition(
            ScrollProperties scrollBar, int targetPosition, double totalTimeMs)
        {
            if (totalTimeMs <= 0.0)
            {
                throw new ArgumentException();
            }

            int initPos = scrollBar.Value;
            if (targetPosition == initPos) return;
            int pos = initPos;
            var sw = new Stopwatch();
            var timer = new Timer();
            timer.Interval = 5;
            timer.Tick += (s, e) =>
            {
                long t = sw.ElapsedMilliseconds;

                if (t >= totalTimeMs)
                {
                    sw.Stop();
                    timer.Stop();
                    timer.Dispose();
                    scrollBar.Value = targetPosition;
                }
                else
                {
                    var stepTarget = (Sin(t / totalTimeMs * PI - PI / 2.0) + 1)
                    / 2.0 * (targetPosition - initPos) + initPos;
                    scrollBar.Value = (int)stepTarget;
                }
            };

            timer.Start();
            sw.Start();
        }
    }
}
