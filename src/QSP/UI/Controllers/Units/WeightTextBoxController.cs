using QSP.Utilities.Units;
using System;
using System.Windows.Forms;
using static QSP.AviationTools.Constants;

namespace QSP.UI.Controllers.Units
{
    // Use this abstraction so that there is no need to worry about weight units.
    // Sync the weight unit with the text in TextBox.
    public class WeightTextBoxController
    {
        public TextBox TxtBox { get; }
        public Label Lable { get; }
        private string format;
        private WeightUnit _unit;

        public WeightTextBoxController(
            TextBox txtBox,
            Label lable = null,
            string format = "F0",             // Display as integer
            WeightUnit unit = WeightUnit.KG)
        {
            this.TxtBox = txtBox;
            this.Lable = lable;
            this.format = format;
            this._unit = unit;
        }

        /// <exception cref="InvalidOperationException"></exception>
        public double GetWeightKg()
        {
            if (double.TryParse(TxtBox.Text.Trim(), out var num))
            {
                if (_unit == WeightUnit.KG) return num;
                return num * LbKgRatio;
            }

            throw new InvalidOperationException();
        }

        public void SetWeight(double weightKg)
        {
            double num = weightKg;

            if (_unit == WeightUnit.LB)
            {
                // Display in LB.
                num *= KgLbRatio;
            }

            TxtBox.Text = num.ToString(format);
        }

        public WeightUnit Unit
        {
            get => _unit;

            set
            {
                if (value == _unit) return;

                var wt = TryGetWtKg();
                _unit = value;

                if (Lable != null)
                {
                    Lable.Text = Conversions.WeightUnitToString(_unit);
                }

                if (wt != null) SetWeight(wt.Value);
            }
        }

        private double? TryGetWtKg()
        {
            try
            {
                return GetWeightKg();
            }
            catch
            {
                return null;
            }
        }
    }
}
