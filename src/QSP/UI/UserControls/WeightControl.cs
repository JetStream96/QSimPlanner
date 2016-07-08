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
        private AcConfigManager configs;

        public WeightControl()
        {
            InitializeComponent();
        }

        public void Init(AcConfigManager config, double zfwKg)
        {
            this.configs = config;
            SetInitState(zfwKg);
        }

        private void SetInitState(double zfwKg)
        {
            zfwKg = Min(zfwKg, configs.MaxZfwKg);
            payloadTrackBar.SetRange(
                0, (int)Ceiling(configs.MaxZfwKg - configs.OewKg));
            var payload = RoundToInt(zfwKg - configs.OewKg);
            wtUnitComboBox.SelectedIndex = 0;
            oewTxtBox.Text = RoundToInt(configs.OewKg).ToString();
            payloadTxtBox.Text = payload.ToString();
            payloadTrackBar.Value = payload;
            zfwTxtBox.Text = RoundToInt(zfwKg).ToString();
        }


    }
}
