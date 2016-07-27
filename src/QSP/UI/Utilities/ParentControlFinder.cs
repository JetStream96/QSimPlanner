using System.Windows.Forms;

namespace QSP.UI.Utilities
{
    public static class ParentControlFinder
    {
        /// <summary>
        /// Find the parent panel of control.
        /// </summary>
        public static Panel FindPanel(this Control control)
        {
            do
            {
                control = control.Parent;
            } while (control is Panel == false);

            return (Panel)control;
        }
    }
}
