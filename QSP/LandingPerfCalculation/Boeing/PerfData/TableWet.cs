using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LandingPerfCalculation.Boeing.PerfData
{
    public class TableWet
    {
        private double[][][][] dataWet;

        public TableWet(double[][][][] dataWet)
        {
            this.dataWet = dataWet;
        }

        public double GetValue(
            int FlapIndex,
            SurfaceCondition SurfCond,
            int BrakeIndex,
            DataColumn column)
        {
            return dataWet[FlapIndex][(int)SurfCond - 1][BrakeIndex][(int)column];
        }
    }
}
