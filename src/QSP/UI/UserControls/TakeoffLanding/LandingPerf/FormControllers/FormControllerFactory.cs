using System;
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
        public static FormController GetController(ControllerType type, 
            AircraftConfigItem ac, PerfTable acPerf, LandingPerfElements elements)
        {
            switch (type)
            {
                case ControllerType.Boeing:
                    return new BoeingController(ac, acPerf, elements);

                default:
                    throw new ArgumentException();
            }
        }
    }
}
