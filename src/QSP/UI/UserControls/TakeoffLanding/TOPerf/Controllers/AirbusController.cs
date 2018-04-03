using QSP.TOPerfCalculation;
using QSP.TOPerfCalculation.Airbus;
using QSP.TOPerfCalculation.Airbus.DataClasses;
using System.Linq;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public class AirbusController : IFormController
    {
        private FormControllerData controllerData;

        public FormOptions Options
        {
            get
            {
                return new FormOptions()
                {
                    Packs = Arr("ON", "OFF"),
                    AntiIces = Arr("OFF", "ONLY ENG A/I", "ENG AND WING A/I"),
                    Flaps = PerfTable.AvailableFlaps().ToArray(),
                    Surfaces = Arr("Dry", "Wet"),
                    Derates = Arr("TO")
                };
            }
        }

        private AirbusPerfTable PerfTable => (AirbusPerfTable)controllerData.PerfTable.Item;

        public AirbusController(FormControllerData d)
        {
            this.controllerData = d;
        }

        public TOReport GetReport(TOParameters p, double tempIncrementCelsius)
        {
            return Calculator.TakeOffReport(PerfTable, p, tempIncrementCelsius);
        }
    }
}
