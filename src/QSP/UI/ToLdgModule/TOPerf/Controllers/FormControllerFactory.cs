using QSP.TOPerfCalculation;
using System;

namespace QSP.UI.ToLdgModule.TOPerf.Controllers
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
            TOPerfElements elements)
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
