﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Controllers
{
    public class ControlDisableStyleController
    {
        private Control btn;
        private Color enabledBack;
        private Color disabledBack;
        private Color enabledFore;
        private Color disabledFore;

        public ControlDisableStyleController(
            Control btn,
            Color enabledBack,
            Color disabledBack,
            Color enabledFore,
            Color disabledFore)
        {
            this.btn = btn;
            this.enabledBack = enabledBack;
            this.disabledBack = disabledBack;
            this.enabledFore = enabledFore;
            this.disabledFore = disabledFore;
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
                btn.ForeColor = enabledFore;
                btn.BackColor = enabledBack;
            }
            else
            {
                btn.ForeColor = disabledFore;
                btn.BackColor = disabledBack;
            }
        }

    }
}