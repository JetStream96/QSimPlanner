using QSP.AviationTools;
using QSP.MathTools.Interpolation;
using System;
using System.Collections.Generic;
using static QSP.Utilities.ConditionChecker;

namespace QSP.WindAloft
{
    public class WindTableCollection : IWindTableCollection
    {
        private IReadOnlyList<int> pressures;
        private WindTable[] windTables;

        public WindTableCollection(WindTable[] windTables)
        {
            pressures = Constants.FullWindDataSet;
            Ensure<ArgumentException>(windTables.Length == pressures.Count);
            this.windTables = windTables;
        }

        public WindUV GetWindUV(double lat, double lon, double altitudeFt)
        {
            double press = CoversionTools.AltToPressureMb(altitudeFt);
            int index = GetIndicesForInterpolation(press);

            double uWind = Interpolate1D.Interpolate(
                pressures[index],
                pressures[index + 1],
                windTables[index].GetUWind(lat, lon),
                windTables[index + 1].GetUWind(lat, lon),
                press);

            double vWind = Interpolate1D.Interpolate(
                pressures[index],
                pressures[index + 1],
                windTables[index].GetVWind(lat, lon),
                windTables[index + 1].GetVWind(lat, lon),
                press);

            return new WindUV(uWind, vWind);
        }

        private int GetIndicesForInterpolation(double press)
        {
            // Let the return value be x, use indices x and x+1 
            // for interpolation.
            // Works for extrapolation as well.

            int len = pressures.Count;

            if (press < pressures[0])
            {
                return 0;
            }

            for (int i = 0; i < len - 1; i++)
            {
                if (press >= pressures[i] &&
                    press <= pressures[i + 1])
                {
                    return i;
                }
            }

            return len - 2;

        }
    }
}
