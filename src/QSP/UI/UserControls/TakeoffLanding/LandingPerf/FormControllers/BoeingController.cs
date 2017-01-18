using System;
using System.Drawing;
using System.Windows.Forms;
using QSP.Common;
using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.LibraryExtension;
using QSP.UI.MsgBox;
using QSP.Utilities.Units;
using QSP.AircraftProfiles.Configs;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public class BoeingController : FormController
    {
        private AircraftConfigItem ac;

        public BoeingController(AircraftConfigItem ac, PerfTable acPerf, LandingPerfElements elem,
            Control parentControl)
            : base(acPerf, elem, parentControl)
        {
            this.ac = ac;
        }

        public override void WeightUnitChanged(object sender, EventArgs e)
        {
            double wt;

            if (double.TryParse(elements.weight.Text, out wt))
            {
                if (elements.wtUnit.SelectedIndex == 0)
                {
                    // LB -> KG 
                    wt *= AviationTools.Constants.LbKgRatio;
                }
                else
                {
                    // KG -> LB
                    wt *= AviationTools.Constants.KgLbRatio;
                }

                elements.weight.Text = ((int)Math.Round(wt)).ToString();
            }
        }

        public override void Initialize()
        {
            SetDefaultSurfCond();
            SetDefaultFlaps();
            SetDefaultRevs();
        }

        public override void SurfCondChanged(object sender, EventArgs e)
        {
            UpdateBrks();
        }

        private void SetDefaultSurfCond()
        {
            var items = elements.surfCond.Items;

            items.Clear();
            items.AddRange(new[] {
                "Dry",
                "Good Braking Action",
                "Medium Braking Action",
                "Poor Braking Action"});

            elements.surfCond.SelectedIndex = 0;
        }

        private void SetDefaultFlaps()
        {
            var items = elements.flaps.Items;
            items.Clear();

            foreach (var i in ((BoeingPerfTable)acPerf.Item).Flaps)
            {
                items.Add(i);
            }

            elements.flaps.SelectedIndex = items.Count - 1;
        }


        private void SetDefaultRevs()
        {
            var items = elements.reverser.Items;
            items.Clear();

            foreach (var i in ((BoeingPerfTable)acPerf.Item).Reversers)
            {
                items.Add(i);
            }

            elements.reverser.SelectedIndex = 0;
        }

        private void UpdateBrks()
        {
            var brks = elements.brake;

            string oldSetting = brks.Text;
            brks.Items.Clear();

            var availableBrakes = ((BoeingPerfTable)acPerf.Item)
                .BrakesAvailable((SurfaceCondition)elements.surfCond.SelectedIndex);

            foreach (var i in availableBrakes)
            {
                brks.Items.Add(i);
            }

            int matchIndex = Array.FindIndex(availableBrakes, x => x == oldSetting);
            brks.SelectedIndex = Math.Max(0, matchIndex);
        }

        public override void Compute(object sender, EventArgs e)
        {
            try
            {
                var para = new BoeingParameterValidator(elements).Validate();
                if (!CheckWeight(para)) return;

                var report = new LandingReportGenerator((BoeingPerfTable)acPerf.Item, para)
                             .GetReport();

                var text = report.ToString((LengthUnit)elements.lengthUnit.SelectedIndex);

                // To center the text in the richTxtBox
                elements.result.Text = text.ShiftToRight(14);

                OnCalculationComplete(EventArgs.Empty);
                elements.result.ForeColor = Color.Black;
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
