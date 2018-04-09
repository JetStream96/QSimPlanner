using QSP.TOPerfCalculation;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System.Linq;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public class BoeingController : IFormController
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
                    Flaps = PerfTable.Flaps.ToArray(),
                    Surfaces = Arr("Dry", "Wet"),
                    Derates = GetDerates()
                };
            }
        }

        private BoeingPerfTable PerfTable => (BoeingPerfTable)controllerData.PerfTable.Item;

        private string[] GetDerates()
        {
            var e = controllerData.Elements;
            var thrustComboBox = e.ThrustRating;
            if (e.Flaps.SelectedIndex < 0) return Arr("TO");
            var table = PerfTable.Tables[e.Flaps.SelectedIndex];
            if (table.AltnRatingAvail) return table.ThrustRatings.ToArray();
            return Arr("TO");
        }

        public BoeingController(FormControllerData d)
        {
            this.controllerData = d;
        }

        public TOReport GetReport(TOParameters p, double tempIncrementCelsius)
        {
            return new TOReportGenerator(PerfTable, p).TakeOffReport(tempIncrementCelsius);
        }
    }
}
