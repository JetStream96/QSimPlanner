using System.Drawing;
using System.Windows.Forms;
using QSP.LibraryExtension;
using QSP.UI.Controllers.ControlGroup;
using QSP.UI.Utilities;

namespace QSP.UI.Forms.NavigationBar
{
    public static class MenuController
    {
        public static readonly Color StyleColor = Color.FromArgb(0, 174, 219);

        public static void EnableColorController(Label lbl)
        {
            var back = SystemColors.ButtonHighlight;
            new ControlColorController(lbl, Color.Black, back, StyleColor, back).Subscribed = true;
        }
    }
}