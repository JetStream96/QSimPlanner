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
using System.Linq;
using System.Windows.Forms;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public class AirbusController : IFormController
    {
        private Control parentControl;
        private PerfTable acPerf;
        private TOPerfElements elements;
        private AircraftConfigItem ac;

        public FormOptions Options
        {
            get
            {
                return new FormOptions()
                {
                    Packs = Arr("ON", "OFF"),
                    AntiIces = Arr("OFF", "ONLY ENG A/I", "ENG AND WING A/I"),
                    Flaps = ((AirbusPerfTable)acPerf.Item).AvailableFlaps().ToArray(),
                    Surfaces = Arr("Dry", "Wet"),
                    Derates = Arr("TO")
                };
            }
        }

        public event EventHandler CalculationCompleted;

        public AirbusController(FormControllerData d)
        {
            this.acPerf = d.PerfTable;
            this.elements = d.Elements;
            this.parentControl = d.ParentControl;
            this.ac = d.ConfigItem;
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
