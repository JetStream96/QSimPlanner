using QSP.AircraftProfiles.Configs;
using QSP.UI.Controllers.Units;
using System;
using System.Drawing;
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
        private bool _monitorWeightChange;

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
            _monitorWeightChange = false;
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

                double maxPayload = _aircraftConfig.MaxZfwKg - _aircraftConfig.OewKg;
                payloadTrackBar.SetRange(0, (int)Ceiling(maxPayload));
                oew.SetWeight(AircraftConfig.OewKg);

                try
                {
                    var oldZfw = ZfwKg;
                    ZfwKg = oldZfw;     // Set the value to check bounds.
                    SetControls();
                }
                catch { }
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
                MonitorWeightChange = true;
                zfw.TxtBox.TextChanged += ChangeColor;
                _enabled = true;
            }
        }

        public void Disable()
        {
            if (_enabled)
            {
                MonitorWeightChange = false;
                zfw.TxtBox.TextChanged -= ChangeColor;
                _enabled = false;
            }
        }

        private bool MonitorWeightChange
        {
            get { return _monitorWeightChange; }

            set
            {
                if (value && _monitorWeightChange == false)
                {
                    payload.TxtBox.TextChanged += PayloadChanged;
                    payloadTrackBar.ValueChanged += TrackBarChanged;
                    zfw.TxtBox.TextChanged += ZfwChanged;
                    _monitorWeightChange = value;
                }
                else if (value == false && _monitorWeightChange)
                {
                    payload.TxtBox.TextChanged -= PayloadChanged;
                    payloadTrackBar.ValueChanged -= TrackBarChanged;
                    zfw.TxtBox.TextChanged -= ZfwChanged;
                    _monitorWeightChange = value;
                }
            }
        }

        private void ChangeColor(object sender, EventArgs e)
        {
            const double margin = 1.0;

            try
            {
                if (AircraftConfig.OewKg <= ZfwKg + margin &&
                    ZfwKg <= AircraftConfig.MaxZfwKg + margin)
                {
                    zfw.TxtBox.ForeColor = Color.DarkGreen;
                }
                else
                {
                    zfw.TxtBox.ForeColor = Color.Red;
                }
            }
            catch
            {
                zfw.TxtBox.ForeColor = Color.Black;
            }
        }

        private void PayloadChanged(object sender, EventArgs e)
        {
            MonitorWeightChange = false;

            try
            {
                var payloadKg = payload.GetWeightKg();

                if (payloadKg >= 0.0)
                {
                    zfw.SetWeight(oew.GetWeightKg() + payload.GetWeightKg());
                    payloadTrackBar.Value = RoundToInt(payload.GetWeightKg());
                }
            }
            catch { }

            MonitorWeightChange = true;
        }

        private void TrackBarChanged(object sender, EventArgs e)
        {
            MonitorWeightChange = false;

            try
            {
                payload.SetWeight(payloadTrackBar.Value);
                zfw.SetWeight(oew.GetWeightKg() + payload.GetWeightKg());
            }
            catch { }

            MonitorWeightChange = true;
        }

        private void ZfwChanged(object sender, EventArgs e)
        {
            MonitorWeightChange = false;

            try
            {
                var payloadKg = zfw.GetWeightKg() - oew.GetWeightKg();

                if (payloadKg >= 0.0)
                {
                    payload.SetWeight(payloadKg);
                    payloadTrackBar.Value = RoundToInt(payloadKg);
                }
            }
            catch { }

            MonitorWeightChange = true;
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
