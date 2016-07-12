using System.Windows.Forms;
using System.Drawing;

namespace QSP.UI.Utilities
{
    public static class LocationGetter
    {
        /// <summary>
        /// Get the location of control relative to parent form's (0, 0).
        /// </summary>
        public static Point LocationInForm(this Control control)
        {
            var pt = Point.Empty;

            while(control is Form == false)
            {
                pt += (Size)control.Location;
                control = control.Parent;
            }

            return pt;
        }
    }
}
