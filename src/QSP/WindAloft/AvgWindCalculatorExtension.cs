using System.Collections.Generic;
using QSP.RouteFinding.Data.Interfaces;

namespace QSP.WindAloft
{
    public static class AvgWindCalculatorExtension
    {
        public static double GetAirDistance(
            this AvgWindCalculator calc,
            IEnumerable<ICoordinate> route)
        {
            double airDis = 0.0;
            ICoordinate last = null;

            foreach (var i in route)
            {
                if (last != null) airDis += calc.GetAirDistance(last, i);
                last = i;
            }

            return airDis;
        }
    }
}
