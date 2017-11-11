using System.Drawing;
using QSP.UI.Util;

namespace QSP.UI.Models
{
    public static class ButtonColorStyle
    {
        public static ControlDisableStyleController.ColorStyle Default => 
            new ControlDisableStyleController.ColorStyle(
                Color.DarkSlateGray,
                Color.FromArgb(224, 224, 224),
                Color.White,
                Color.LightGray);
    }
}