using QSP.AviationTools;
using QSP.MathTools.Interpolation;
using System;
using System.Collections.Generic;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.WindAloft
{
    public class WxTableCollection : IWxTableCollection
    {
        private IReadOnlyList<int> pressures;
        private WxTable[] windTables;

        public WxTableCollection(WxTable[] windTables)
        {
            pressures = Constants.FullWindDataSet;
            Ensure<ArgumentException>(windTables.Length == pressures.Count);
            this.windTables = windTables;
        }

        private double InterpolatePressure(double lat, double lon, double altFt,
            Func<WxTable, TableItem> getTable)
        {
            double press = ConversionTools.PressureMb(altFt);
            int index = GetIndicesForInterpolation(press);
            return Interpolate1D.Interpolate(
                pressures[index],
                pressures[index + 1],
                getTable(windTables[index]).ValueAt(lat, lon),
                getTable(windTables[index + 1]).ValueAt(lat, lon),
                press);
        }

        public double GetTemp(double lat, double lon, double altitudeFt)
        {
            return InterpolatePressure(lat, lon, altitudeFt, t => t.Temperature);
        }

        public WindUV GetWindUV(double lat, double lon, double altitudeFt)
        {
            double u = InterpolatePressure(lat, lon, altitudeFt, t => t.UWind);
            double v = InterpolatePressure(lat, lon, altitudeFt, t => t.VWind);
            return new WindUV(u, v);
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
