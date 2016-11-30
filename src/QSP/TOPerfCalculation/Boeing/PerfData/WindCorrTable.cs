using System.Linq;
using QSP.MathTools.TablesNew;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class WindCorrTable
    {
        public Table Table;

        public WindCorrTable(Table Table)
        {
            this.Table = Table;
        }

        public WindCorrTable(
            double[] slopeCorrectedLengths,
            double[] winds,
            double[][] windCorrectedLength)
        {
            Table = TableBuilder.Build2D(slopeCorrectedLengths, winds, windCorrectedLength);
        }

        public double CorrectedLength(double slopeCorrectedLength, double wind)
        {
            return Table.ValueAt(slopeCorrectedLength, wind);
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
            var slopeCorrLengths = Table.XValues;
            var windCorrLengths = slopeCorrLengths.Select(x => Table.ValueAt(x, headwindComponent));
            return TableBuilder.Build1D(windCorrLengths.ToList(), slopeCorrLengths);
        }
    }
}
