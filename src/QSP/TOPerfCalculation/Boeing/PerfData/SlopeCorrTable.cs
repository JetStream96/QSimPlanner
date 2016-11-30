using System.Linq;
using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class SlopeCorrTable : Table2D
    {
        public SlopeCorrTable(double[] physicalLengths,
            double[] slopes,
            double[][] correctedLenth)
            : base(physicalLengths, slopes, correctedLenth)
        { }

        public double CorrectedLength(double physicalLength, double slope)
        {
            return ValueAt(physicalLength, slope);
        }

        public double FieldLengthRequired(double slope, double slopeCorrectedLength)
        {
            return TableFieldLength(slope).ValueAt(slopeCorrectedLength);
        }

        // Maps sloped corrected length into field length.
        private Table1D TableFieldLength(double slope)
        {
            return new Table1D(x.Select(i => ValueAt(i, slope)).ToArray(), x);
        }        
    }
}
