using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public class BoeingController : IFormController
    {
        private ControllerData controllerData;

        public FormOptions Options
        {
            get
            {
                var item = ((BoeingPerfTable)controllerData.PerfTable.Item);
                var surfaceIndex = controllerData.Elements.SurfCond.SelectedIndex;

                // When the control is loading, surfaceIndex might be -1.
                var brakes = surfaceIndex >= 0 ?
                    item.BrakesAvailable((SurfaceCondition)surfaceIndex) :
                    Arr("Manual");

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
                    Brakes = brakes
                };
            }
        }

        public BoeingController(ControllerData controllerData)
        {
            this.controllerData = controllerData;
        }

        public LandingReport GetReport(LandingParameters p)
        {
            var item = (BoeingPerfTable)controllerData.PerfTable.Item;
            return new LandingReportGenerator(item, p).GetReport();
        }
    }
}
