using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.AircraftProfiles.Configs;
using QSP.AircraftProfiles;
using static QSP.MathTools.Doubles;
using static System.Math;

namespace QSP.UI.UserControls
{
    public partial class WeightControl : UserControl
    {
        private AircraftConfigItem config;

        public WeightControl()
        {
            InitializeComponent();
        }

        public void Init(AircraftConfigItem config, double zfwKg)
        {
            this.config = config;
            SetInitState(zfwKg);
        }

        private void SetInitState(double zfwKg)
        {
            zfwKg = Min(zfwKg, config.MaxZfwKg);
            payloadTrackBar.SetRange(
                0, (int)Ceiling(config.MaxZfwKg - config.OewKg));
            var payload = RoundToInt(zfwKg - config.OewKg);
            wtUnitComboBox.SelectedIndex = 0;
            oewTxtBox.Text = RoundToInt(config.OewKg).ToString();
            payloadTxtBox.Text = payload.ToString();
            payloadTrackBar.Value = payload;
            zfwTxtBox.Text = RoundToInt(zfwKg).ToString();
        }


    }
}
