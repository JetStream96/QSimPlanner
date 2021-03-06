﻿using System;
using System.Windows.Forms;
using QSP.AviationTools;
using QSP.UI.UserControls.TakeoffLanding.Common;
using QSP.UI.Util;
using QSP.Utilities.Units;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf
{
    public partial class CustomFuelForm : Form
    {
        private AircraftRequest acRequest;

        public double LandingWtKg { get; private set; }
        public event EventHandler WeightSet;

        public CustomFuelForm()
        {
            InitializeComponent();
        }

        public void Init(AircraftRequest acRequest)
        {
            this.acRequest = acRequest;
            landingFuelTxtBox.Text = "0";
            wtUnitLbl.Text = Conversions.WeightUnitToString(acRequest.WtUnit);
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void importFuelBtn_Click(object sender, EventArgs e)
        {
            LandingWtKg = acRequest.LandingWeightKg;
            Close();
            WeightSet?.Invoke(this, EventArgs.Empty);
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (double.TryParse(landingFuelTxtBox.Text, out var fuel) &&
                fuel >= 0.0)
            {
                if (acRequest.WtUnit == WeightUnit.LB)
                {
                    fuel *= Constants.LbKgRatio;
                }

                LandingWtKg = acRequest.ZfwKg + fuel;
                Close();
                WeightSet?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                this.ShowWarning("The landing fuel is not valid.");
            }
        }
    }
}
