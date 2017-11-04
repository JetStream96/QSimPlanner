using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public class ControlDisableStyleController
    {
        private readonly Control btn;
        private readonly ColorStyle style;

        public ControlDisableStyleController(Control btn, ColorStyle style)
        {
            this.btn = btn;
            this.style = style;
        }
        
        public void Activate()
        {
            btn.EnabledChanged += ChangeAppearance;
            ChangeAppearance(null, null);
        }

        public void Deactivate()
        {
            btn.EnabledChanged -= ChangeAppearance;
        }

        private void ChangeAppearance(object sender, EventArgs e)
        {
            if (btn.Enabled)
            {
                btn.ForeColor = style.EnabledFore;
                btn.BackColor = style.EnabledBack;
            }
            else
            {
                btn.ForeColor = style.DisabledFore;
                btn.BackColor = style.DisabledBack;
            }

            btn.Refresh();
        }

        public class ColorStyle
        {
            public Color EnabledBack { get; private set; }
            public Color DisabledBack { get; private set; }
            public Color EnabledFore { get; private set; }
            public Color DisabledFore { get; private set; }

            public ColorStyle(
                Color EnabledBack,
                Color DisabledBack,
                Color EnabledFore,
                Color DisabledFore)
            {
                this.EnabledBack = EnabledBack;
                this.DisabledBack = DisabledBack;
                this.EnabledFore = EnabledFore;
                this.DisabledFore = DisabledFore;
            }
        }
    }
}
