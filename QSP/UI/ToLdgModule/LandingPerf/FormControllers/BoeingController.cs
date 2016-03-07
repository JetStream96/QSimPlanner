using QSP.Core;
using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.LandingPerf.FormControllers
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
                    wt *= AviationTools.Constants.LB_KG;
                }
                else
                {
                    // KG -> LB
                    wt *= AviationTools.Constants.KG_LB;
                }

                elements.weight.Text = ((int)Math.Round(wt)).ToString();
            }
        }

        public override void Initialize()
        {
            setDefaultSurfCond();
            setDefaultFlaps();
            setDefaultRevs();
        }

        public override void SurfCondChanged(object sender, EventArgs e)
        {
            updateBrks();
        }

        private void setDefaultSurfCond()
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

        private void setDefaultFlaps()
        {
            var items = elements.flaps.Items;
            items.Clear();

            foreach (var i in ((BoeingPerfTable)acPerf.Item).FlapsAvailable())
            {
                items.Add(i);
            }

            elements.flaps.SelectedIndex = items.Count - 1;
        }


        private void setDefaultRevs()
        {
            var items = elements.reverser.Items;
            items.Clear();

            foreach (var i in ((BoeingPerfTable)acPerf.Item).RevAvailable())
            {
                items.Add(i);
            }

            elements.reverser.SelectedIndex = 0;
        }

        private void updateBrks()
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

                var result = new LandingPerfCalculation.Boeing.Calculator(
                    (BoeingPerfTable)acPerf.Item).GetLandingReport(para);

                elements.result.Text = result.ToString(
                    elements.lengthUnit.SelectedIndex == 0 ?
                    LengthUnit.Meter :
                    LengthUnit.Feet);

                //    formStateManagerLDG.Save();
            }
            catch (InvalidUserInputException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (RunwayTooShortException)
            {
                elements.result.ForeColor = Color.Red;
                elements.result.Text = "Runway length is insufficient for landing.";
            }
        }
    }
}
