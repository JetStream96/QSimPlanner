using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class WindCorrTable : Table2D
    {
        public WindCorrTable(
            double[] slopeCorrectedLengths,
            double[] winds,
            double[][] windCorrectedLength)
            : base(slopeCorrectedLengths, winds, windCorrectedLength)
        { }

        public double CorrectedLength(double slopeCorrectedLength, double wind)
        {
            return ValueAt(slopeCorrectedLength, wind);
        }

        public double SlopeCorrectedLength(double headwind, double WindAndSlopeCorrectedLength)
        {
            return TableSlopeCorrLength(headwind).ValueAt(WindAndSlopeCorrectedLength);
        }

        // A table maps (Wind and slope corrected runway length) to 
        // (Slope corrected runway length).
        //
        private Table1D TableSlopeCorrLength(double headwindComponent)
        {
            var slopeCorrLength = new double[x.Length];

            for (int i = 0; i < slopeCorrLength.Length; i++)
            {
                slopeCorrLength[i] = ValueAt(x[i], headwindComponent);
            }

            return new Table1D(slopeCorrLength, x);
        }
    }
}
