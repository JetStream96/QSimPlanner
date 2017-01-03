using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ControlGroup
{
    public class ControlSwitcher
    {
        private bool _subscribed;
        private Panel outerPanel;

        public IEnumerable<ControlPair> Pairings { get; private set; }

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
                    foreach (var i in Pairings)
                    {
                        i.Button.Click -= ShowControl;
                    }
                    _subscribed = false;
                }
                else if (_subscribed == false && value)
                {
                    foreach (var i in Pairings)
                    {
                        i.Button.Click += ShowControl;
                    }
                    _subscribed = true;
                }
            }
        }

        public ControlSwitcher(Panel outerPanel, params ControlPair[] pairings)
        {
            this.outerPanel = outerPanel;
            this.Pairings = pairings;
            _subscribed = false;
        }

        private void ShowControl(object sender, EventArgs e)
        {
            foreach (var i in Pairings)
            {
                i.Control.Visible = (i.Button == sender);
            }

            // Workaround for windows scrollbar bug. Without this the 
            // scrollbars will appear even if the form is large enough to
            // fit the contents.
            outerPanel.AutoScroll = false;
            outerPanel.HorizontalScroll.Visible = false;
            outerPanel.VerticalScroll.Visible = false;
            outerPanel.AutoScroll = true;
        }

        public class ControlPair
        {
            public Control Button { get; private set; }
            public UserControl Control { get; private set; }

            public ControlPair(Control Button, UserControl Control)
            {
                this.Button = Button;
                this.Control = Control;
            }
        }
    }
}
