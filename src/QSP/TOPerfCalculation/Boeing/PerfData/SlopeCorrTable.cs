using System.Linq;
using QSP.MathTools.Tables;
using QSP.MathTools.TablesNew;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class SlopeCorrTable
    {
        public Table Table { get; }

        public SlopeCorrTable(Table Table)
        {
            this.Table = Table;
        }

        public SlopeCorrTable(double[] physicalLengths, double[] slopes, double[][] correctedLenth)
        {
            Table = TableBuilder.Build2D(physicalLengths, slopes, correctedLenth);
        }

        public double CorrectedLength(double physicalLength, double slope)
        {
            return Table.ValueAt(physicalLength, slope);
        }

        public double FieldLengthRequired(double slope, double slopeCorrectedLength)
        {
            return TableFieldLength(slope).ValueAt(slopeCorrectedLength);
        }

        // Maps sloped corrected length into physical field length.
        private Table TableFieldLength(double slope)
        {
            var physicalLength = Table.XValues;
            var fieldLengths = physicalLength.Select(x => Table.ValueAt(x, slope));
            return TableBuilder.Build1D(fieldLengths.ToList(), physicalLength);
        }
    }
}
