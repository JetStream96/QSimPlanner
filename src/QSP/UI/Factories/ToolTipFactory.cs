using System.Windows.Forms;

namespace QSP.UI.Factories
{
    public static class ToolTipFactory
    {
        public static ToolTip GetToolTip()
        {
            var tp = new ToolTip();

            tp.AutoPopDelay = 5000;
            tp.InitialDelay = 0;
            tp.ReshowDelay = 0;
            tp.ShowAlways = true;

            return tp;
        }
    }
}
