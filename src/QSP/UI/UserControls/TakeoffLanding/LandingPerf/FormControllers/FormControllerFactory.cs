using System;
using QSP.LandingPerfCalculation;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
{
    public enum ControllerType
    {
        Boeing
    }

    public static class FormControllerFactory
    {
        public static FormController GetController(
            ControllerType type,
            PerfTable acPerf,
            LandingPerfElements elements)
        {
            switch (type)
            {
                case ControllerType.Boeing:
                    return new BoeingController(acPerf, elements);

                default:
                    throw new ArgumentException();
            }
        }
    }
}
