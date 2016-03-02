using QSP.LandingPerfCalculation;
using QSP.Core;

namespace QSP.UI.Forms.LandingPerf.FormControllers
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
                    throw new EnumNotSupportedException();
            }
        }
    }
}
