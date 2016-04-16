using QSP.LibraryExtension;

namespace QSP.LandingPerfCalculation.Boeing.PerfData
{
    public class TableDry
    {
        private double[][][] dataDry;

        public TableDry(double[][][] dataDry)
        {
            this.dataDry = dataDry;
        }

        public double GetValue(int FlapIndex, int BrakeIndex, DataColumn column)
        {
            return dataDry[FlapIndex][BrakeIndex][(int)column];
        }

        public bool Equals(TableDry item, double delta)
        {
            return DoubleArrayCompare.Equals(dataDry, item.dataDry, delta);
        }
    }
}
