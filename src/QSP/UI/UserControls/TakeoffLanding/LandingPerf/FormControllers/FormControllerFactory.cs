using System;
using System.Windows.Forms;
using QSP.AircraftProfiles.Configs;
using QSP.LandingPerfCalculation;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public enum ControllerType
    {
        Boeing
    }

    public static class FormControllerFactory
    {
        public static IFormController GetController(ControllerType type, AircraftConfigItem ac, 
            PerfTable acPerf, LandingPerfElements elements, Control parentControl)
        {
            switch (type)
            {
                case ControllerType.Boeing:
                    return new BoeingController(ac, acPerf, elements, parentControl);

                default:
                    throw new ArgumentException();
            }
        }
    }
}
