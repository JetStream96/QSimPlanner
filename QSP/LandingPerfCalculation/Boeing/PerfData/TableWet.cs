using QSP.LibraryExtension;

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

        public bool Equals(TableWet item, double delta)
        {
            return DoubleArrayCompare.Equals(dataWet, item.dataWet, delta);
        }
    }
}
