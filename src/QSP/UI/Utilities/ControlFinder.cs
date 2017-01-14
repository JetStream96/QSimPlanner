using System.Windows.Forms;

namespace QSP.UI.Utilities
{
    public static class ControlFinder
    {
        /// <summary>
        /// Find the parent panel of control.
        /// </summary>
        public static Panel FindPanel(this Control control)
        {
            do
            {
                control = control.Parent;
            } while (!(control is Panel) || control is TableLayoutPanel);

            return (Panel)control;
        }
    }
}
