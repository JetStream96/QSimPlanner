using QSP.AircraftProfiles.Configs;
using QSP.UI.Controllers.Units;
using System;
using System.Drawing;
using MetroFramework.Controls;
using static QSP.MathTools.Numbers;
using static System.Math;

namespace QSP.UI.Controllers.WeightControl
{
    public class WeightController
    {
        private WeightTextBoxController oew;
        private WeightTextBoxController payload;
        private WeightTextBoxController zfw;
        private MetroTrackBar payloadTrackBar;
        private bool _enabled;
        private bool _monitorWeightChange;
        private double _oewKg;
        private double _maxZfwKg;

        public WeightController(
            WeightTextBoxController oew,
            WeightTextBoxController payload,
            WeightTextBoxController zfw,
            MetroTrackBar payloadTrackBar)
        {
            this.oew = oew;
            this.payload = payload;
            this.zfw = zfw;
            this.payloadTrackBar = payloadTrackBar;
            _enabled = false;
            _monitorWeightChange = false;
        }

        public void SetAircraftWeights(double oewKg, double maxZfwKg)
        {
            _oewKg = oewKg;
            _maxZfwKg = maxZfwKg;

            double maxPayload = maxZfwKg - oewKg;
            payloadTrackBar.Maximum = (int)Ceiling(maxPayload);
            payloadTrackBar.Minimum = 0;
            oew.SetWeight(oewKg);

            try
            {
                var oldZfw = ZfwKg;
                ZfwKg = oldZfw;     // Set the value to check bounds.
                SetControls();
            }
            catch { }
        }

        // Set this after setting AircraftConfig.        
        public double ZfwKg
        {
            // Can throw InvalidOperationException if the weight is not a number.
            get
            {
                return zfw.GetWeightKg();
            }

            // Can throw NullReferenceException.
            set
            {
                if (value > _maxZfwKg)
                {
                    zfw.SetWeight(_maxZfwKg);
                }
                else if (value < _oewKg)
                {
                    zfw.SetWeight(_oewKg);
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
                if (_oewKg <= ZfwKg + margin &&
                    ZfwKg <= _maxZfwKg + margin)
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
            var payloadKg = ZfwKg - _oewKg;
            oew.SetWeight(_oewKg);
            payload.SetWeight(payloadKg);
            payloadTrackBar.Value = RoundToInt(payloadKg);
            zfw.SetWeight(ZfwKg);
        }
    }
}
