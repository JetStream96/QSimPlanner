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
    }
}
