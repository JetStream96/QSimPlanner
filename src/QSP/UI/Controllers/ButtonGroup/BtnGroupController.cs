using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Controllers.ButtonGroup
{
    public class BtnGroupController
    {
        private BtnColorController[] controllers;

        public BtnGroupController(
            Color foreInactive,
            Color backInactive,
            Color foreActive,
            params BtnColorPair[] btnColors)
        {
            controllers = new BtnColorController[btnColors.Length];

            for (int i = 0; i < controllers.Length; i++)
            {
                controllers[i] = new BtnColorController(
                    btnColors[i].Btn,
                    foreInactive,
                    backInactive,
                    foreActive,
                    btnColors[i].BackActive);
            }
        }
        
        public void Initialize()
        {
            foreach (var i in controllers)
            {
                i.Button.FlatAppearance.BorderSize = 0;
                i.Subscribed = true;
                i.SetInactiveStyle();
                i.Button.Click += setSelected;
            }
        }

        public void SetSelected(Button btn)
        {
            foreach (var i in controllers)
            {
                if (i.Button == btn)
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
            SetSelected((Button)sender);
        }

        public class BtnColorPair
        {
            public Button Btn { get; private set; }
            public Color BackActive { get; private set; }

            public BtnColorPair(Button Btn,Color Color)
            {
                this.Btn = Btn;
                this.BackActive = Color;
            }
        }
    }
}
