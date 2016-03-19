using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class WindCorrTable
    {
        private Table2D table;

        public WindCorrTable(Table2D table)
        {
            this.table = table;
        }

        public double CorrectedLength(double slopeCorrectedLength, double wind)
        {
            return table.ValueAt(slopeCorrectedLength, wind);
        }
    }
}
