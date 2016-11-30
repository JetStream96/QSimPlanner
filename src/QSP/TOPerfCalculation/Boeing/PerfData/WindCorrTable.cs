using System.Linq;
using QSP.MathTools.Tables;
using QSP.MathTools.TablesNew;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class WindCorrTable
    {
        private readonly Table table;

        public WindCorrTable(
            double[] slopeCorrectedLengths,
            double[] winds,
            double[][] windCorrectedLength)
        {
            table = TableBuilder.Build2D(slopeCorrectedLengths, winds, windCorrectedLength);
        }

        public double CorrectedLength(double slopeCorrectedLength, double wind)
        {
            return table.ValueAt(slopeCorrectedLength, wind);
        }

        public double SlopeCorrectedLength(double headwind, double WindAndSlopeCorrectedLength)
        {
            return TableSlopeCorrLength(headwind).ValueAt(WindAndSlopeCorrectedLength);
        }

        // A table maps (Wind and slope corrected runway length) to 
        // (Slope corrected runway length).
        //
        private Table TableSlopeCorrLength(double headwindComponent)
        {
            var slopeCorrLengths = table.XValues;
            var windCorrLengths = slopeCorrLengths.Select(x => table.ValueAt(x, headwindComponent));
            return TableBuilder.Build1D(windCorrLengths.ToList(), slopeCorrLengths);
        }
    }
}
