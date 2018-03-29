using QSP.AircraftProfiles.Configs;
using QSP.LibraryExtension;
using QSP.TOPerfCalculation;
using QSP.TOPerfCalculation.Airbus;
using QSP.TOPerfCalculation.Airbus.DataClasses;
using QSP.UI.Models.MsgBox;
using QSP.UI.Util;
using QSP.Utilities.Units;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public class AirbusController : IFormController
    {
        private Control parentControl;
        private PerfTable acPerf;
        private TOPerfElements elements;
        private AircraftConfigItem ac;

        public event EventHandler CalculationCompleted;

        public AirbusController(AircraftConfigItem ac, PerfTable acPerf, TOPerfElements elements,
            Control parentControl)
        {
            this.acPerf = acPerf;
            this.elements = elements;
            this.parentControl = parentControl;
            this.ac = ac;
        }

        public void WeightUnitChanged(object sender, EventArgs e)
        {
            if (double.TryParse(elements.Weight.Text, out var wt))
            {
                if (elements.WtUnit.SelectedIndex == 0)
                {
                    // LB -> KG 
                    wt *= AviationTools.Constants.LbKgRatio;
                }
                else
                {
                    // KG -> LB
                    wt *= AviationTools.Constants.KgLbRatio;
                }

                elements.Weight.Text = ((int)Math.Round(wt)).ToString();
            }
        }

        public void Initialize()
        {
            SetDefaultSurfCond();
            SetDefaultFlaps();
            SetDerate();
            SetPackOptions();
            SetAntiIceOptions();
        }

        private void SetPackOptions()
        {
            var items = elements.Packs.Items;
            items.Clear();
            items.AddRange(new[] { "ON", "OFF" });
            elements.Packs.SelectedIndex = 0;
        }

        private void SetAntiIceOptions()
        {
            var items = elements.AntiIce.Items;
            items.Clear();
            items.AddRange(new[]
            {
                "OFF",
                "ONLY ENG A/I",
                "ENG AND WING A/I"
            });
            elements.AntiIce.SelectedIndex = 0;
        }

        public void FlapsChanged(object sender, EventArgs e)
        {
            SetDerate();
        }

        private void SetDerate()
        {
            var thrustComboBox = elements.ThrustRating;

            var items = thrustComboBox.Items;
            items.Clear();

            items.Add("TO");
            thrustComboBox.Enabled = false;

            thrustComboBox.SelectedIndex = 0;
        }

        private void SetDefaultSurfCond()
        {
            var surf = elements.SurfCond;
            var items = surf.Items;
            var old = surf.Text;

            items.Clear();
            items.AddRange(new[] { "Dry", "Wet" });
            surf.SelectedIndex = 0;
            surf.Text = old;
        }

        private void SetDefaultFlaps()
        {
            var items = elements.Flaps.Items;
            items.Clear();

            foreach (var i in ((AirbusPerfTable)acPerf.Item).AvailableFlaps())
            {
                items.Add(i);
            }

            elements.Flaps.SelectedIndex = 0;
        }

        // Returns whether continue to calculate.
        private bool CheckWeight(TOParameters para)
        {
            if (para.WeightKg > ac.MaxTOWtKg || para.WeightKg < ac.OewKg)
            {
                var result = parentControl.ShowDialog(
                    "Takeoff weight is not within valid range. Continue to calculate?",
                    MsgBoxIcon.Warning,
                    "",
                    DefaultButton.Button2,
                    "Yes", "No");

                return result == MsgBoxResult.Button1;
            }

            return true;
        }

        // Returns null if failed.
        private TOParameters TryGetPara()
        {
            try
            {
                return ParameterValidator.Validate(elements);
            }
            catch (Exception ex)
            {
                parentControl.ShowWarning(ex.Message);
                return null;
            }
        }

        public void Compute(object sender, EventArgs e)
        {
            try
            {
                var para = TryGetPara();
                if (para == null) return;
                var table = (AirbusPerfTable)acPerf.Item;
                if (!CheckWeight(para)) return;
                var tempUnit = (TemperatureUnit)elements.TempUnit.SelectedIndex;
                var tempIncrement = tempUnit == TemperatureUnit.Celsius ? 1.0 : 2.0 / 1.8;
                var (err, report) = Calculator.TakeOffReport(table, para, tempIncrement);

                if (err == Calculator.Error.RunwayTooShort)
                {
                    parentControl.ShowWarning("Runway length is insufficient for takeoff.");
                    return;
                }

                if (err != Calculator.Error.None)
                {
                    parentControl.ShowError("An unexpected error occurred.");
                    return;
                }

                var text = report.ToString(tempUnit, (LengthUnit)elements.lengthUnit.SelectedIndex);

                // To center the text in the richTxtBox
                elements.Result.Text = text.ShiftToRight(20);

                CalculationCompleted?.Invoke(this, EventArgs.Empty);
                elements.Result.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                parentControl.ShowWarning(ex.Message);
            }
        }
    }
}