using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ButtonGroup
{
    public class BtnColorController
    {
        public Button Button { get; private set; }
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
                    Button.MouseEnter -= SetActiveStyle;
                    Button.MouseLeave -= SetInactiveStyle;
                    _subscribed = false;
                }
                else if (_subscribed == false && value)
                {
                    Button.MouseEnter += SetActiveStyle;
                    Button.MouseLeave += SetInactiveStyle;
                    _subscribed = true;
                }
            }
        }

        public BtnColorController(
            Button Button,
            Color foreInactive,
            Color backInactive,
            Color foreActive,
            Color backActive)
        {
            this.Button = Button;
            this.foreInactive = foreInactive;
            this.backInactive = backInactive;
            this.foreActive = foreActive;
            this.backActive = backActive;
            _subscribed = false;
        }

        public void SetInactiveStyle(object sender = null, EventArgs e = null)
        {
            Button.ForeColor = foreInactive;
            Button.BackColor = backInactive;
        }

        public void SetActiveStyle(object sender = null, EventArgs e = null)
        {
            Button.ForeColor = foreActive;
            Button.BackColor = backActive;
        }
    }
}
