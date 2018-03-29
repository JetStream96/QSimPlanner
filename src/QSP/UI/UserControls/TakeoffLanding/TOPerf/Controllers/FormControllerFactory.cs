using System;
using System.Windows.Forms;
using QSP.AircraftProfiles.Configs;
using QSP.TOPerfCalculation;
using QSP.TOPerfCalculation.Airbus.DataClasses;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public static class FormControllerFactory
    {
        public static IFormController GetController(AircraftConfigItem ac,
            PerfTable acPerf, TOPerfElements elements, Control parentControl)
        {
            var item = acPerf.Item;

            if (item is BoeingPerfTable)
            {
                return new BoeingController(ac, acPerf, elements, parentControl);
            }

            if (item is AirbusPerfTable)
            {
                return new AirbusController(ac, acPerf, elements, parentControl);
            }

            throw new ArgumentException();
        }
    }
}
