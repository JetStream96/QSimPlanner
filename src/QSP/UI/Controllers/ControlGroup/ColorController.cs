using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ControlGroup
{
    public class ColorController
    {
        public Control Control { get; private set; }
        private Color foreInactive;
        private Color backInactive;
        private Color foreActive;
        private Color backActive;
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

        public ColorController(
            Control Control,
            Color foreInactive,
            Color backInactive,
            Color foreActive,
            Color backActive)
        {
            this.Control = Control;
            this.foreInactive = foreInactive;
            this.backInactive = backInactive;
            this.foreActive = foreActive;
            this.backActive = backActive;
            _subscribed = false;
        }

        public void SetInactiveStyle(object sender = null, EventArgs e = null)
        {
            Control.ForeColor = foreInactive;
            Control.BackColor = backInactive;
        }

        public void SetActiveStyle(object sender = null, EventArgs e = null)
        {
            Control.ForeColor = foreActive;
            Control.BackColor = backActive;
        }
    }
}
