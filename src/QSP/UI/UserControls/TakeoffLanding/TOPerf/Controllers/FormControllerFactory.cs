using System;
using QSP.AircraftProfiles.Configs;
using QSP.TOPerfCalculation;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public enum ControllerType
    {
        Boeing
    }

    public static class FormControllerFactory
    {
        public static FormController GetController(
            ControllerType type,
            AircraftConfigItem ac,
            PerfTable acPerf,
            TOPerfElements elements)
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
