using System.Windows.Forms;
namespace QSP
{
    public class TablessControl : TabControl
	{

		protected override void WndProc(ref Message m)
		{
			// Hide tabs by trapping the TCM_ADJUSTRECT message
			if (m.Msg == 0x1328 && !DesignMode) {
				m.Result = new System.IntPtr(1);
			} else {
				base.WndProc(ref m);
			}
		}

	}
}
