using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.AircraftProfiles.Configs;
using QSP.AircraftProfiles;
using static System.Math;
using static QSP.MathTools.Doubles;
using QSP.Utilities.Units;

namespace QSP.UI.Controllers.WeightControl
{
    public class WeightController
    {
        private TextBox oewTxtBox;
        private TextBox payloadTxtBox;
        private TextBox zfwTxtBox;
        private TrackBar payloadTrackBar;
        private bool _enabled;
        private AircraftConfigItem config;
        private WeightUnit _weightUnit;

        public WeightController(
            TextBox oewTxtBox,
            TextBox payloadTxtBox,
            TextBox zfwTxtBox,
            TrackBar payloadTrackBar)
        {
            this.oewTxtBox = oewTxtBox;
            this.payloadTxtBox = payloadTxtBox;
            this.zfwTxtBox = zfwTxtBox;
            this.payloadTrackBar = payloadTrackBar;
            _enabled = false;
        }

        /// <summary>
        /// Subscribe or unsubsribe the event handlers.
        /// </summary>
        public bool Enabled
        {
            get
            {

            }

            set
            {

            }
        }

        public WeightUnit WeightUnit
        {
            get
            {

            }

            set
            {

            }
        }

        private void Enable()
        {

        }

        public void SetAircraft(AircraftConfigItem config, double zfwKg)
        {
            zfwKg = Min(zfwKg, config.MaxZfwKg);
            payloadTrackBar.SetRange(
                0, (int)Ceiling(config.MaxZfwKg - config.OewKg));
            var payload = RoundToInt(zfwKg - config.OewKg);
            _weightUnit = WeightUnit.KG;
            oewTxtBox.Text = RoundToInt(config.OewKg).ToString();
            payloadTxtBox.Text = payload.ToString();
            payloadTrackBar.Value = payload;
            zfwTxtBox.Text = RoundToInt(zfwKg).ToString();
        }
    }
}
