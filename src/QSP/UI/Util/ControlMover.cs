using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public static class ControlMover
    {
        public static void MoveRight(this Control control, int step)
        {
            var loc = control.Location;
            var newLoc = new Point(loc.X + step, loc.Y);
            control.Location = newLoc;
        }

        public static void MoveDown(this Control control, int step)
        {
            var loc = control.Location;
            var newLoc = new Point(loc.X, loc.Y + step);
            control.Location = newLoc;
        }

        public static void MoveRight(this IEnumerable<Control> controls, int step)
        {
            foreach (var i in controls)
            {
                i.MoveRight(step);
            }
        }

        public static void MoveDown(this IEnumerable<Control> controls, int step)
        {
            foreach (var i in controls)
            {
                i.MoveDown(step);
            }
        }
    }
}
