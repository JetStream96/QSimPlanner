using QSP.MathTools.Tables;
using System.Linq;
using static QSP.LibraryExtension.Arrays;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class FieldLimitWtTable
    {
        private Table3D table;

        public FieldLimitWtTable(Table3D table)
        {
            this.table = table;
        }

        public double MaxOat
        {
            get
            {
                return table.z.Last();
            }
        }

        // All weights in ton.
        public double FieldLimitWeight(double pressAlt, double correctedLength, double oat)
        {
            return table.ValueAt(pressAlt, correctedLength, oat);
        }

        public double CorrectedLengthRequired(double altFt,
                                              double oat,
                                              double correctedWtTon)
        {
            return tableComputeRwyRequired(altFt, oat).ValueAt(correctedWtTon);
        }

        // A table maps TO weights (ton) to rwy length required.
        private Table1D tableComputeRwyRequired(double altitudeFt, double oat)
        {
            double[] weights = new double[table.y.Length];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = FieldLimitWeight(altitudeFt, table.y[i], oat);
            }

            return new Table1D(weights, table.y);
        }
    }
}
