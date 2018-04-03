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
                var surface = (SurfaceCondition)controllerData.Elements.SurfCond.SelectedIndex;
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
