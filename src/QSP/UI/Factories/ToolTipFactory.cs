using System.Windows.Forms;

namespace QSP.UI.Factories
{
    public static class ToolTipFactory
    {
        public static ToolTip GetToolTip()
        {
            var tp = new ToolTip();

            tp.AutoPopDelay = 5000;
            tp.InitialDelay = 1000;
            tp.ReshowDelay = 500;
            tp.ShowAlways = true;

            return tp;
        }
    }
}
