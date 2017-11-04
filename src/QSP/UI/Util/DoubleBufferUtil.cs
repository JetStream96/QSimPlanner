using System.Reflection;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public static class DoubleBufferUtil
    {
        // Some controls, like TableLayoutPanel, does not have a public property 'DoubleBuffered'.
        // That can cause flickering problems. This method uses reflection to enable 
        // double buffer.
        //
        public static void SetDoubleBuffered(Control c)
        {
            if (SystemInformation.TerminalServerSession) return;

            PropertyInfo propInfo = typeof(Control)
                .GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            propInfo.SetValue(c, true, null);
        }
    }
}