using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Airbus;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public class AirbusController : IFormController
    {
        private ControllerData controllerData;

        public FormOptions Options
        {
            get
            {
                var item = ((AirbusPerfTable)controllerData.PerfTable.Item);
                return new FormOptions()
                {
                    SurfaceConditions = Arr("Dry", "Wet"),
                    Flaps = item.AllFlaps(),
                    Reversers = Arr("No reverser", "Both reversers"),
                    Brakes = item.AllBrakes()
                };
            }
        }

        public AirbusController(ControllerData controllerData)
        {
            this.controllerData = controllerData;
        }

        public LandingReport GetReport(LandingParameters p)
        {
            return Calculator.LandingReport(new CalculatorData()
            {
                Parameters = p,
                Table = (AirbusPerfTable)controllerData.PerfTable.Item
            });
        }
    }
}
