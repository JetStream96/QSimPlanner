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

        public double MaxOat
        {
            get
            {
                return z.Last();
            }
        }

        // All weights in ton.
        public double FieldLimitWeight(double pressAlt, double correctedLength, double oat)
        {
            return ValueAt(pressAlt, correctedLength, oat);
        }

        public double CorrectedLengthRequired(double altFt,
                                              double oat,
                                              double fieldLimitWtTon)
        {
            return tableComputeRwyRequired(altFt, oat).ValueAt(fieldLimitWtTon);
        }

        // A table maps TO weights (ton) to rwy length required.
        private Table1D tableComputeRwyRequired(double altitudeFt, double oat)
        {
            double[] weights = new double[y.Length];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = FieldLimitWeight(altitudeFt, y[i], oat);
            }

            return new Table1D(weights, y);
        }        
    }
}
