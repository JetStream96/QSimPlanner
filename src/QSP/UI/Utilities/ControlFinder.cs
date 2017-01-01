using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<T> GetAllControls<T>(this Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAllControls<T>(ctrl))
                           .Concat(controls.Where(c => c is T).Cast<T>());
        }
    }
}
