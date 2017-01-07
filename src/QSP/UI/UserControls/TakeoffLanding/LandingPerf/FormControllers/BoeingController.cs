using System;
using System.Drawing;
using QSP.Common;
using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.LibraryExtension;
using QSP.UI.MsgBox;
using QSP.Utilities.Units;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public class BoeingController : FormController
    {
        public BoeingController(PerfTable acPerf, LandingPerfElements elements)
            : base(acPerf, elements)
        { }

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
            items.AddRange(new object[] {
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

                var report = new LandingPerfCalculation
                             .Boeing
                             .LandingReportGenerator(
                                 (BoeingPerfTable)acPerf.Item,
                                 para)
                             .GetReport();

                var text = report.ToString(
                elements.lengthUnit.SelectedIndex == 0 ?
                LengthUnit.Meter :
                LengthUnit.Feet);

                // To center the text in the richTxtBox
                elements.result.Text = text.ShiftToRight(14);

                OnCalculationComplete(EventArgs.Empty);
                elements.result.ForeColor = Color.Black;
            }
            catch (InvalidUserInputException ex)
            {
                MsgBoxHelper.ShowWarning(ex.Message);
            }
            catch (RunwayTooShortException)
            {
                MsgBoxHelper.ShowWarning("Runway length is insufficient for landing.");
            }
        }
    }
}
