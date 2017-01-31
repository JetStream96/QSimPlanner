using QSP.MathTools.Interpolation;
using System;
using static QSP.LibraryExtension.Arrays;

namespace QSP.WindAloft
{
    public class WindTable
    {
        // The unit is knots.
        private double[,] uTable;
        private double[,] vTable;

        public WindTable(double[,] uTable, double[,] vTable)
        {
            this.uTable = uTable;
            this.vTable = vTable;
        }

        public double GetUWind(double lat, double lon)
        {
            return GetUVWindHelper(lat, lon, TableOption.U);
        }

        public double GetVWind(double lat, double lon)
        {
            return GetUVWindHelper(lat, lon, TableOption.V);
        }

        public enum TableOption
        {
            U,
            V
        }

        private double GetUVWindHelper(double lat, double lon, TableOption para)
        {
            var x = (int) (Math.Floor(lat));
            var y = (int) (Math.Floor(lon));

            // Prevent interpolation using data that is out of range of array.
            if (x == 90) x = 89;
            if (y == 180) y = 179;

            return Interpolate2D.Interpolate(
                new double[] {x, x + 1},
                new double[] {y, y + 1},
                new[] {
                    new[] {GetWindHelper(x, y, para), GetWindHelper(x, y + 1, para)},
                    new[] {GetWindHelper(x + 1, y, para), GetWindHelper(x + 1, y + 1, para)}
                },
                x, y);
        }

        private double GetWindHelper(int lat, int lon, TableOption para)
        {
            return para == TableOption.U ?
                 uTable[lat + 90, lon + 180] :
                 vTable[lat + 90, lon + 180];
        }
    }
}
