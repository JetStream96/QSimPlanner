using System.Drawing;

namespace QSP.UI.Controllers.ControlGroup
{
    public class ColorGroup
    {
        public Color ForeInactive { get; }
        public Color BackInactive { get; }
        public Color ForeHover { get; }
        public Color BackHover { get; }
        public Color ForeActive { get; }
        public Color BackActive { get; }

        public ColorGroup(
            Color foreInactive,
            Color backInactive,
            Color foreHover,
            Color backHover,
            Color foreActive,
            Color backActive)
        {
            this.ForeInactive = foreInactive;
            this.BackInactive = backInactive;
            this.ForeHover = foreHover;
            this.BackHover = backHover;
            this.ForeActive = foreActive;
            this.BackActive = backActive;
        }
    }
}