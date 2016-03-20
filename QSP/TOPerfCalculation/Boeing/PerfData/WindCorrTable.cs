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

        public double SlopeCorrectedLength(double headwind, double WindAndSlopeCorrectedLength)
        {
            return tableSlopeCorrLength(headwind).ValueAt(WindAndSlopeCorrectedLength);
        }

        // A table maps (Wind and slope corrected runway length) to 
        // (Slope corrected runway length).
        //
        private Table1D tableSlopeCorrLength(double headwindComponent)
        {
            var slopeCorrLength = new double[table.x.Length];

            for (int i = 0; i < slopeCorrLength.Length; i++)
            {
                slopeCorrLength[i] = table.ValueAt(table.x[i], headwindComponent);
            }

            return new Table1D(slopeCorrLength, table.x);
        }
    }
}
