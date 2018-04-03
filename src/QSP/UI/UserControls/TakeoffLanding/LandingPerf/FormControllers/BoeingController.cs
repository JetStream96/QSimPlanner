using QSP.AircraftProfiles.Configs;
using QSP.Common;
using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.LibraryExtension;
using QSP.UI.Models.MsgBox;
using QSP.UI.Util;
using QSP.Utilities.Units;
using System;
using System.Drawing;
using System.Windows.Forms;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public class BoeingController : IFormController
    {
        private PerfTable acPerf;
        private LandingPerfElements elements;
        private Control parentControl;
        private AircraftConfigItem ac;

        public FormOptions Options
        {
            get
            {
                var item = ((BoeingPerfTable)acPerf.Item);
                var surface = (SurfaceCondition)elements.SurfCond.SelectedIndex;
                return new FormOptions()
                {
                    SurfaceConditions = Arr(
                        "Dry",
                        "Good Braking Action",
                        "Medium Braking Action",
                        "Poor Braking Action"
                    ),
                    Flaps = item.Flaps,
                    Reversers = item.Reversers,
                    Brakes = item.BrakesAvailable(surface)
                };
            }
        }

        public event EventHandler CalculationCompleted;

        public BoeingController(AircraftConfigItem ac, PerfTable acPerf,
            LandingPerfElements elements, Control parentControl)
        {
            this.acPerf = acPerf;
            this.elements = elements;
            this.parentControl = parentControl;
            this.ac = ac;
        }

        public void Compute(object sender, EventArgs e)
        {
            try
            {
                var para = new ParameterValidator(elements).Validate();
                if (!CheckWeight(para)) return;

                var report = new LandingReportGenerator((BoeingPerfTable)acPerf.Item, para)
                             .GetReport();

                var text = report.ToString((LengthUnit)elements.LengthUnit.SelectedIndex);

                // To center the text in the richTxtBox
                elements.Result.Text = text.ShiftToRight(15);

                CalculationCompleted?.Invoke(this, EventArgs.Empty);
                elements.Result.ForeColor = Color.Black;
            }
            catch (InvalidUserInputException ex)
            {
                parentControl.ShowWarning(ex.Message);
            }
            catch (RunwayTooShortException)
            {
                parentControl.ShowWarning("Runway length is insufficient for landing.");
            }
        }

        // Returns whether continue to calculate.
        private bool CheckWeight(LandingParameters para)
        {
            if (para.WeightKG > ac.MaxTOWtKg || para.WeightKG < ac.OewKg)
            {
                var result = parentControl.ShowDialog(
                    "Landing weight is not within valid range. Continue to calculate?",
                    MsgBoxIcon.Warning,
                    "",
                    DefaultButton.Button2,
                    "Yes", "No");

                return result == MsgBoxResult.Button1;
            }

            return true;
        }
    }
}
