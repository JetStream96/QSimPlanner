using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ControlGroup
{
    public class ColorController
    {
        public Control Control { get; private set; }
        private ColorGroup colors;
        private bool _subscribed;

        public bool Subscribed
        {
            get
            {
                return _subscribed;
            }

            set
            {
                if (_subscribed && value == false)
                {
                    Control.MouseEnter -= SetActiveStyle;
                    Control.MouseLeave -= SetInactiveStyle;
                    _subscribed = false;
                }
                else if (_subscribed == false && value)
                {
                    Control.MouseEnter += SetActiveStyle;
                    Control.MouseLeave += SetInactiveStyle;
                    _subscribed = true;
                }
            }
        }

        public ColorController(Control Control, ColorGroup colors)
        {
            this.Control = Control;
            this.colors = colors;
            _subscribed = false;
        }

        public void SetInactiveStyle(object sender = null, EventArgs e = null)
        {
            Control.ForeColor = colors.ForeInactive;
            Control.BackColor = colors.BackInactive;
        }

        public void SetActiveStyle(object sender = null, EventArgs e = null)
        {
            Control.ForeColor = colors.ForeActive;
            Control.BackColor = colors.BackActive;
        }
    }
}
