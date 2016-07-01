using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Factories
{
    public static class FormFactory
    {
        public static Form GetForm(Size size)
        {
            var frm = new Form();
            frm.Size = size;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.White;
            frm.AutoScaleMode = AutoScaleMode.Dpi;
            frm.ShowInTaskbar = false;

            return frm;
        }
    }
}
