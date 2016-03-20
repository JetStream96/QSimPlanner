using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class SlopeCorrTable
    {
        private Table2D table;

        public SlopeCorrTable(Table2D table)
        {
            this.table = table;
        }

        public double CorrectedLength(double physicalLength, double slope)
        {
            return table.ValueAt(physicalLength, slope);
        }

        public double FieldLengthRequired(double slope,double slopeCorrectedLength)
        {
            return tableFieldLength(slope).ValueAt(slopeCorrectedLength);
        }

        // Maps sloped corrected length into field length.
        private Table1D tableFieldLength(double slope)
        {
            double[] fieldLength = new double[table.x.Length];

            for (int i = 0; i < fieldLength.Length; i++)
            {
                fieldLength[i] = table.ValueAt(table.x[i], slope);
            }

            return new Table1D(fieldLength, table.x);
        }
    }
}
