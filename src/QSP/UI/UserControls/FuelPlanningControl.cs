using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.Controllers.Units;
using QSP.UI.Controllers.WeightControl;

namespace QSP.UI.UserControls
{
    public partial class FuelPlanningControl : UserControl
    {
        private WeightTextBoxController oew;
        private WeightTextBoxController payload;
        private WeightTextBoxController zfw;
        private WeightController weightControl;

        public FuelPlanningControl()
        {
            InitializeComponent();
        }

        public void Init()
        {

        }

        private void SetWeightController()
        {
            oew = new WeightTextBoxController(oewTxtBox);
            payload = new WeightTextBoxController(payloadTxtBox);
            zfw = new WeightTextBoxController(zfwTxtBox);

            weightControl = new WeightController(
                oew, payload, zfw, payloadTrackBar);
            // weightControl.AircraftConfig = ??
            // weightControl.ZfwKg = ??
        }
    }
}
