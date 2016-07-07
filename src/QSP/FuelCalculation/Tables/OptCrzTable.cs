using QSP.AviationTools;
using QSP.MathTools.Tables;
using QSP.MathTools.Tables.Readers;
using QSP.Utilities.Units;
using System;

namespace QSP.FuelCalculation.Tables
{
    public class OptCrzTable
    {
        private Table1D table;

        public OptCrzTable(string text, WeightUnit unit)
        {
            Func<string, double> weightParser = (s) =>
            {
                var wt = double.Parse(s);

                if (unit == WeightUnit.LB)
                {
                    wt *= Constants.LbKgRatio;
                }

                return wt;
            };

            table = TableReader1D.Read(text, weightParser, double.Parse);
        }
        
        public double OptimumAltitudeFt(double wtTon)
        {
            return table.ValueAt(wtTon);
        }

        public double DistanceLimitedAltitudeFt(double distanceNm)
        {
            return distanceNm / 2 * 1000 / 3;
        }

        public double ActualCrzAltFt(double wtTon, double disNm)
        {
            //FT
            return Math.Min(OptimumAltitudeFt(wtTon),
                DistanceLimitedAltitudeFt(disNm));
        }
    }
}
