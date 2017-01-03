using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ControlGroup
{
    public class GroupController
    {
        private List<ColorController> controllers;

        public GroupController(params ControlColorPair[] controlColors)
        {
            controllers = controlColors.Select(c => new ColorController(c.Control, c.Colors))
                .ToList();
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
            public Control Control { get; }
            public ColorGroup Colors { get; }

            public ControlColorPair(Control Control, ColorGroup colors)
            {
                this.Control = Control;
                this.Colors = colors;
            }
        }
    }
}
