using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Utilities
{
    public static class ScrollBars
    {
        public static bool VScrollBarVisible(this Control c)
        {
            return c.Controls.OfType<VScrollBar>().First().Visible;
        }
    }
}