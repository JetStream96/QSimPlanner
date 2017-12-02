using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public static class ControlDrawing
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control c)
        {
            SendMessage(c.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control c)
        {
            SendMessage(c.Handle, WM_SETREDRAW, true, 0);
            c.Refresh();
        }

        public static void SuspendDrawingWhen(this Control c, Action a)
        {
            SuspendDrawing(c);
            a();
            ResumeDrawing(c);
        }
    }
}
