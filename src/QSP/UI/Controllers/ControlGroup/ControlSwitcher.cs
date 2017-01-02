using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ControlGroup
{
    public class ControlSwitcher
    {
        private ControlControlPair[] _pairings;
        private bool _subscribed;
        private Panel panel;

        public IEnumerable<ControlControlPair> Pairings
        {
            get { return _pairings; }
        }

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
                    foreach (var i in _pairings)
                    {
                        i.Button.Click -= ShowControl;
                    }
                    _subscribed = false;
                }
                else if (_subscribed == false && value)
                {
                    foreach (var i in _pairings)
                    {
                        i.Button.Click += ShowControl;
                    }
                    _subscribed = true;
                }
            }
        }

        public ControlSwitcher(Panel panel, params ControlControlPair[] pairings)
        {
            this.panel = panel;
            this._pairings = pairings;
            _subscribed = false;
        }

        private void ShowControl(object sender, EventArgs e)
        {
            foreach (var i in _pairings)
            {
                i.Control.Visible = (i.Button == sender);
            }

            // Workaround for windows scrollbar bug. Without this the 
            // scrollbars will appear even if the form is large enough to
            // fit the contents.
            panel.AutoScroll = false;
            panel.HorizontalScroll.Visible = false;
            panel.VerticalScroll.Visible = false;
            panel.AutoScroll = true;
        }

        public class ControlControlPair
        {
            public Control Button { get; private set; }
            public UserControl Control { get; private set; }

            public ControlControlPair(Control Button, UserControl Control)
            {
                this.Button = Button;
                this.Control = Control;
            }
        }
    }
}
