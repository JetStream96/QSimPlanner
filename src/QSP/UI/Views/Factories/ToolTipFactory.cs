using System.Windows.Forms;

namespace QSP.UI.Views.Factories
{
    public static class ToolTipFactory
    {
        public static ToolTip GetToolTip()
        {
            var tp = new ToolTip();

            tp.UseAnimation = true;
            tp.AutoPopDelay = 60000;
            tp.InitialDelay = 0;
            tp.ReshowDelay = 0;
            tp.ShowAlways = true;

            return tp;
        }

        public static void SetToolTip(this Control c, string caption)
        {
            GetToolTip().SetToolTip(c, caption);
        }
    }
}
