using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Views
{
    public class PanelSilentScrollbar : Panel
    {
        // To prevent AutoScroll from change ScrollBar's value property.
        protected override Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }
    }
}
