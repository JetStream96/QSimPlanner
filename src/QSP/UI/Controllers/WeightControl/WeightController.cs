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
        private AircraftConfigItem _aircraftConfig;
        private bool _enabled;

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

                var oldZfw = ZfwKg;
                ZfwKg = oldZfw;     // Set the value to check bounds.
                SetControls();
            }
        }

        // Set this after setting AircraftConfig.        
        public double ZfwKg
        {
            // Can throw InvalidOperationException.
            get
            {
                return zfw.GetWeightKg();
            }

            // Can throw NullReferenceException.
            set
            {
                if (value > AircraftConfig.MaxZfwKg)
                {
                    zfw.SetWeight(AircraftConfig.MaxZfwKg);
                }
                else if (value < AircraftConfig.OewKg)
                {
                    zfw.SetWeight(AircraftConfig.OewKg);
                }
                else
                {
                    zfw.SetWeight(value);
                }
            }
        }

        public void Enable()
        {
            if (_enabled == false)
            {
                payload.TxtBox.TextChanged += PayloadChanged;
                payloadTrackBar.ValueChanged += TrackBarChanged;
                zfw.TxtBox.TextChanged += ZfwChanged;
                _enabled = true;
            }
        }

        public void Disable()
        {
            if (_enabled)
            {
                payload.TxtBox.TextChanged -= PayloadChanged;
                payloadTrackBar.ValueChanged -= TrackBarChanged;
                zfw.TxtBox.TextChanged -= ZfwChanged;
                _enabled = false;
            }
        }

        private void PayloadChanged(object sender, EventArgs e)
        {
            try
            {
                zfw.SetWeight(oew.GetWeightKg() + payload.GetWeightKg());
                payloadTrackBar.Value = RoundToInt(payload.GetWeightKg());
            }
            catch { }
        }

        private void TrackBarChanged(object sender, EventArgs e)
        {
            try
            {
                payload.SetWeight(payloadTrackBar.Value);
                zfw.SetWeight(oew.GetWeightKg() + payload.GetWeightKg());
            }
            catch { }
        }

        private void ZfwChanged(object sender, EventArgs e)
        {
            try
            {
                payload.SetWeight(zfw.GetWeightKg() - oew.GetWeightKg());
                payloadTrackBar.Value = RoundToInt(payload.GetWeightKg());
            }
            catch { }
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
