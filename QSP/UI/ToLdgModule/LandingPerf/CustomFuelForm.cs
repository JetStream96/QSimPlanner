using QSP.AviationTools;
using QSP.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.LandingPerf
{
    public partial class CustomFuelForm : Form
    {
        private WeightUnit _wtUnit;

        public double ZfwKg { get; set; }
        public double PredictedFuelKg { get; set; }

        public WeightUnit WtUnit
        {
            get
            {
                return _wtUnit;
            }

            set
            {
                _wtUnit = value;
                wtUnitLbl.Text = _wtUnit == WeightUnit.KG ?
                    "KG" :
                    "LB";
            }
        }

        public double LandingWtKg
        {
            get
            {
                return ZfwKg +
                    double.Parse(landingFuelTxtBox.Text) *
                    (_wtUnit == WeightUnit.KG ? 1.0 : Constants.LbKgRatio);
            }
        }

        public CustomFuelForm()
        {
            InitializeComponent();

            landingFuelTxtBox.Text = "0";
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void importFuelBtn_Click(object sender, EventArgs e)
        {
            double fuelDisplay = PredictedFuelKg *
                (_wtUnit == WeightUnit.KG ? 1.0 : Constants.KgLbRatio);

            landingFuelTxtBox.Text = ((int)Math.Round(fuelDisplay)).ToString();
        }
    }
}
