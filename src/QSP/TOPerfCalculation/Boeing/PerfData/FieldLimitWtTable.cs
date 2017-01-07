using QSP.MathTools.Tables;
using System.Linq;
using static QSP.LibraryExtension.Arrays;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class FieldLimitWtTable : Table3D
    {
        public FieldLimitWtTable(double[] pressAlts,
            double[] correctedLengths,
            double[] oats,
            double[][][] fieldLimWt)
            : base(pressAlts, correctedLengths, oats, fieldLimWt)
        { }

        public double MaxOat => z.Last();

        // All weights in ton.
        public double FieldLimitWeight(double pressAlt, double correctedLength, double oat)
        {
            return ValueAt(pressAlt, correctedLength, oat);
        }

        public double CorrectedLengthRequired(double altFt, double oat, double fieldLimitWtTon)
        {
            return TableComputeRwyRequired(altFt, oat).ValueAt(fieldLimitWtTon);
        }

        // A table maps TO weights (ton) to rwy length required.
        private Table1D TableComputeRwyRequired(double altitudeFt, double oat)
        {
            double[] weights = y.Select(i => FieldLimitWeight(altitudeFt, i, oat)).ToArray();
            return new Table1D(weights, y).TruncateRepeatedXValues();
        }
    }
}
