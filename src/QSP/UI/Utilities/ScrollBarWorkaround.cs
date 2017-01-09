using System.Windows.Forms;

namespace QSP.UI.Utilities
{
    public static class ScrollBarWorkaround
    {
        // Workaround for windows scrollbar bug. Without this the 
        // scrollbars will appear even if the form is large enough to
        // fit the contents.
        //
        // To reproduce, place a tab control on a panel, which has AutoScroll = True.
        // Let tab control contain tab 1 and 2. Now when tab 1 is showing.
        // Resize the width of the form until the horizontal scroll bar shows up.
        // Then increase the width again until the horizontal scroll bar disappears.
        // Switch to tab 2, and then back to tab 1. The horizontal scroll bar will remain visible
        // and will continue to exist even if the form width is increased further.
        // (This is only tested on my Windows 7.)
        //
        // This should be called whenever the scroll bar content resizes.
        public static void RefreshScrollBar(Panel p)
        {
            p.AutoScroll = false;
            p.HorizontalScroll.Visible = false;
            p.VerticalScroll.Visible = false;
            p.AutoScroll = true;
        }
    }
}