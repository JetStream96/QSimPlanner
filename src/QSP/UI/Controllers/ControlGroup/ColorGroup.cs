using System.Drawing;

namespace QSP.UI.Controllers.ControlGroup
{
    public class ColorGroup
    {
        public Color ForeInactive { get; }
        public Color BackInactive { get; }
        public Color ForeActive { get; }
        public Color BackActive { get; }

        public ColorGroup(
            Color foreInactive,
            Color backInactive,
            Color foreActive,
            Color backActive)
        {
            this.ForeInactive = foreInactive;
            this.BackInactive = backInactive;
            this.ForeActive = foreActive;
            this.BackActive = backActive;
        }
    }
}