using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.UI.Controllers.Units;
using QSP.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;
using static System.Math;

namespace QSP.UI.Controllers.WeightControl
{
    public class WeightController
    {
        private WeightTextBoxController oew;
        private WeightTextBoxController payload;
        private WeightTextBoxController zfw;
        private TrackBar payloadTrackBar;
        private bool _enabled;
        private double _zfwKg;
        private AircraftConfigItem _aircraftConfig;

        public WeightController(
            WeightTextBoxController oew,
            WeightTextBoxController payload,
            WeightTextBoxController zfw,
            TrackBar payloadTrackBar)
        {
            this.oew = oew;
            this.payload = payload;
            this.zfw = zfw;
            this.payloadTrackBar = payloadTrackBar;
            _enabled = false;
        }

        public AircraftConfigItem AircraftConfig
        {
            get
            {
                return _aircraftConfig;
            }
            set
            {
                _aircraftConfig = value;

                double maxPayload =
                    _aircraftConfig.MaxZfwKg - _aircraftConfig.OewKg;
                payloadTrackBar.SetRange(0, (int)Ceiling(maxPayload));
            }
        }

        // Set this after setting AircraftConfig.
        public double ZfwKg
        {
            get
            {
                return _zfwKg;
            }
            set
            {
                _zfwKg = Min(value, AircraftConfig.MaxZfwKg);
            }
        }

        public void Enable()
        {

        }

        public void Disable()
        {

        }

        private void RefreshControls()
        {

        }

        private void SetControls()
        {
            var payloadKg = ZfwKg - AircraftConfig.OewKg;
            oew.SetWeight(AircraftConfig.OewKg);
            payload.SetWeight(payloadKg);
            payloadTrackBar.Value = RoundToInt(payloadKg);
            zfw.SetWeight(ZfwKg);
        }
    }
}
