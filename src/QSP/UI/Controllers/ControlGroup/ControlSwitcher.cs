using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ControlGroup
{
    // Manages a group of UserControls. For each UserControl, when its 
    // corresponding button (or label, or any other controls) is clicked, only that 
    // UserControl is shown and all others are hidden.
    //
    public class ControlSwitcher
    {
        private bool _subscribed;

        public IEnumerable<ControlPair> Pairings { get; private set; }

        public bool Subscribed
        {
            get => _subscribed;

            set
            {
                if (_subscribed && !value)
                {
                    foreach (var i in Pairings)
                    {
                        i.Button.Click -= ShowControl;
                    }
                    _subscribed = false;
                }
                else if (!_subscribed && value)
                {
                    foreach (var i in Pairings)
                    {
                        i.Button.Click += ShowControl;
                    }
                    _subscribed = true;
                }
            }
        }

        public ControlSwitcher(params ControlPair[] pairings)
        {
            this.Pairings = pairings;
            _subscribed = false;
        }

        private void ShowControl(object sender, EventArgs e)
        {
            foreach (var i in Pairings)
            {
                i.Control.Visible = (i.Button == sender);
            }
        }

        /// <summary>
        /// Show the UserControl corresponding to the given Control.
        /// </summary>
        public void ShowControl(Control c)
        {
            ShowControl(c, EventArgs.Empty);
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
