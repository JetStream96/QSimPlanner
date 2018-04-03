using System.Windows.Forms;
using QSP.AircraftProfiles.Configs;
using QSP.LandingPerfCalculation;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public class ControllerData
    {
        public PerfTable PerfTable { get; set; }
        public LandingPerfElements Elements { get; set; }
        public Control ParentControl { get; set; }
        public AircraftConfigItem ConfigItem { get; set; }
    }
}
