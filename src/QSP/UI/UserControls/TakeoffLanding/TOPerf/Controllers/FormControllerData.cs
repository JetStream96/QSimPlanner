using System.Windows.Forms;
using QSP.AircraftProfiles.Configs;
using QSP.TOPerfCalculation;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public class FormControllerData
    {
        public Control ParentControl { get; set; }
        public PerfTable PerfTable { get; set; }
        public TOPerfElements Elements { get; set; }
        public AircraftConfigItem ConfigItem { get; set; }
    }
}