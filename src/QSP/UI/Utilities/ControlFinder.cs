using System;
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

        /// <summary>
        /// Returns the parent form of the given control, or null if no parent form exists.
        /// </summary>
        public static Form ParentForm(this Control control)
        {
            var p = control.Parent;
            if (p == null) return null;
            return p as Form ?? ParentForm(p);
        }
        
        public static Form SelfOrParentForm(this Control control)
        {
            return control as Form ?? control.ParentForm();
        }
    }
}
