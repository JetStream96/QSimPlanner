using QSP.LibraryExtension;
using QSP.AircraftProfiles.Configs;
using QSP.Common;
using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.UI.Models.MsgBox;
using QSP.UI.Util;
using QSP.Utilities.Units;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public class BoeingController : IFormController
    {
        private PerfTable acPerf;
        private LandingPerfElements elements;
        private Control parentControl;
        private AircraftConfigItem ac;

        public event EventHandler CalculationCompleted;

        public BoeingController(AircraftConfigItem ac, PerfTable acPerf,
            LandingPerfElements elements, Control parentControl)
        {
            this.acPerf = acPerf;
            this.elements = elements;
            this.parentControl = parentControl;
            this.ac = ac;
        }

        public void WeightUnitChanged(object sender, EventArgs e)
        {
            if (double.TryParse(elements.weight.Text, out var wt))
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

        public void Initialize()
        {
            SetDefaultSurfCond();
            SetDefaultFlaps();
            SetDefaultRevs();
        }

        public void SurfCondChanged(object sender, EventArgs e)
        {
            UpdateBrks();
        }

        private void SetDefaultSurfCond()
        {
            var surf = elements.surfCond;
            var items = surf.Items;
            var old = surf.Text;

            items.Clear();
            items.AddRange(new[] {
                "Dry",
                "Good Braking Action",
                "Medium Braking Action",
                "Poor Braking Action"});

            surf.SelectedIndex = 0;
            surf.Text = old;
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

        public void Compute(object sender, EventArgs e)
        {
            try
            {
                var para = new BoeingParameterValidator(elements).Validate();
                if (!CheckWeight(para)) return;

                var report = new LandingReportGenerator((BoeingPerfTable)acPerf.Item, para)
                             .GetReport();

                var text = report.ToString((LengthUnit)elements.lengthUnit.SelectedIndex);

                // To center the text in the richTxtBox
                elements.result.Text = text.ShiftToRight(15);

                CalculationCompleted?.Invoke(this, EventArgs.Empty);
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

        // These methods do nothing.
        public void FlapsChanged(object sender, EventArgs e) { }

        public void ReverserChanged(object sender, EventArgs e) { }

        public void BrakesChanged(object sender, EventArgs e) { }
    }
}
