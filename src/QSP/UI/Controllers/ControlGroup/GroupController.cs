using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ControlGroup
{
    public class GroupController
    {
        private ColorController[] controllers;

        public GroupController(params ControlColorPair[] controlColors)
        {
            controllers = new ColorController[controlColors.Length];

            for (int i = 0; i < controllers.Length; i++)
            {
                controllers[i] = new ColorController(
                    controlColors[i].Control,
                    controlColors[i].ForeInactive,
                    controlColors[i].BackInactive,
                    controlColors[i].ForeActive,
                    controlColors[i].BackActive);
            }
        }

        public void Initialize()
        {
            foreach (var i in controllers)
            {
                i.Subscribed = true;
                i.SetInactiveStyle();
                i.Control.Click += setSelected;
            }
        }

        public void SetSelected(Control btn)
        {
            foreach (var i in controllers)
            {
                if (i.Control == btn)
                {
                    i.Subscribed = false;
                    i.SetActiveStyle();
                }
                else
                {
                    i.Subscribed = true;
                    i.SetInactiveStyle();
                }
            }
        }

        private void setSelected(object sender, EventArgs e)
        {
            SetSelected((Control)sender);
        }

        public class ControlColorPair
        {
            public Control Control { get; private set; }
            public Color ForeInactive { get; private set; }
            public Color BackInactive { get; private set; }
            public Color ForeActive { get; private set; }
            public Color BackActive { get; private set; }

            public ControlColorPair(Control Control, Color ForeInactive,
                Color BackInactive, Color ForeActive, Color BackActive)
            {
                this.Control = Control;
                this.ForeInactive = ForeInactive;
                this.BackInactive = BackInactive;
                this.ForeActive = ForeActive;
                this.BackActive = BackActive;
            }

            public ControlColorPair(Control Control, Color[] colors)
                : this(Control, colors[0], colors[1], colors[2], colors[3]) { }
        }
    }
}
