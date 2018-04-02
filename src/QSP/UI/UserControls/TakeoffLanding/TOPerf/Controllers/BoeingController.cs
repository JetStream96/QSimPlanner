using QSP.AircraftProfiles.Configs;
using QSP.Common;
using QSP.LibraryExtension;
using QSP.TOPerfCalculation;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;
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
    public class BoeingController : IFormController
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
                    Flaps = ((BoeingPerfTable)acPerf.Item).Flaps.ToArray(),
                    Surfaces = Arr("Dry", "Wet"),
                    Derates = GetDerates()
                };
            }
        }

        private string[] GetDerates()
        {
            var thrustComboBox = elements.ThrustRating;
            var table = ((BoeingPerfTable)acPerf.Item).Tables[elements.Flaps.SelectedIndex];
            if (table.AltnRatingAvail) return table.ThrustRatings.ToArray();
            return Arr("TO");
        }

        public event EventHandler CalculationCompleted;

        public BoeingController(FormControllerData d)
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

        public void Compute(object sender, EventArgs e)
        {
            try
            {
                var para = ParameterValidator.Validate(elements);
                var table = (BoeingPerfTable)acPerf.Item;
                if (!CheckWeight(para)) return;
                var tempUnit = (TemperatureUnit)elements.TempUnit.SelectedIndex;
                var tempIncrement = tempUnit == TemperatureUnit.Celsius ? 1.0 : 2.0 / 1.8;
                var report = new TOReportGenerator(table, para).TakeOffReport(tempIncrement);

                var text = report.ToString(tempUnit, (LengthUnit)elements.lengthUnit.SelectedIndex);

                // To center the text in the richTxtBox
                elements.Result.Text = text.ShiftToRight(20);

                CalculationCompleted?.Invoke(this, EventArgs.Empty);
                elements.Result.ForeColor = Color.Black;
            }
            catch (InvalidUserInputException ex)
            {
                parentControl.ShowWarning(ex.Message);
            }
            catch (RunwayTooShortException)
            {
                parentControl.ShowWarning("Runway length is insufficient for takeoff.");
            }
            catch (PoorClimbPerformanceException)
            {
                parentControl.ShowWarning("Aircraft too heavy to meet " +
                    "climb performance requirement.");
            }
            catch (Exception ex)
            {
                parentControl.ShowError(ex.Message);
            }
        }
    }
}
