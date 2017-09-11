using System.Linq;
using System.Windows.Forms;
using QSP.LibraryExtension;
using QSP.MathTools;

namespace QSP.UI.Utilities
{
    public static class ScrollBarsUtil
    {
        public static bool VScrollBarVisible(this Control c)
        {
            return c.Controls.OfType<VScrollBar>().First().Visible;
        }

        /// <summary>
        /// Disable all default scroll bar event handlers on any child controls of controlOverride.
        /// All mouse scrolls will cause the scroll bar on showScrollOn to change.
        /// </summary>
        public static void OverrideScrollBar(ScrollableControl showScrollOn,
            Control controlOverride)
        {
            controlOverride.Controls.Cast<Control>().ForEach(
                i => OverrideScrollBar(showScrollOn, i));

            controlOverride.MouseWheel += (s, e) =>
            {
                ((HandledMouseEventArgs)e).Handled = true;
                var scroll = showScrollOn.VerticalScroll;
                if (scroll.Visible)
                {
                    scroll.Value = Numbers.LimitToRange(scroll.Value - e.Delta,
                        scroll.Minimum, scroll.Maximum);
                    showScrollOn.PerformLayout();
                }
            };
        }
    }
}