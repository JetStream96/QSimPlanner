using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.MathTools.Tables;
using static QSP.MathTools.NumericalArrays;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class FieldLimitWtTable
    {
        private Table3D table;

        public FieldLimitWtTable(Table3D table)
        {
            this.table = table;
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
            var lengthArray = table.y;
            bool lengthIsIncreasing = lengthArray.IsIncreasing();
            double limitWt;

            for (int i = 0; i < lengthArray.Length; i++)
            {
                limitWt = FieldLimitWeight(altFt, lengthArray[i], oat);

            }
        }
    }
}
