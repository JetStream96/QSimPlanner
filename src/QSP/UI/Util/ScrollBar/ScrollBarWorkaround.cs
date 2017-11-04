using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Util.ScrollBar
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
    public class ScrollBarWorkaround
    {
        private bool disable = false;
        private readonly Panel panel;
        private Size lastSize;

        public ScrollBarWorkaround(Panel panel)
        {
            this.panel = panel;
            lastSize = panel.ClientSize;
        }

        public void Enable()
        {
            panel.ClientSizeChanged += (s, e) =>
            {
                if (disable || !ShouldFire()) return;
                panel.BeginInvoke((Action)InternalRefresh);
            };
        }

        public void InternalRefresh()
        {
            disable = true;
            panel.AutoScroll = false;
            panel.HorizontalScroll.Visible = false;
            panel.VerticalScroll.Visible = false;
            panel.AutoScroll = true;
            disable = false;
            lastSize = panel.ClientSize;
        }

        private bool ShouldFire()
        {
            var size = panel.ClientSize;
            return Math.Abs(size.Height - lastSize.Height) + Math.Abs(size.Width - lastSize.Width) >= 10;
        }
    }
}