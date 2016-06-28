using QSP.MathTools.Interpolation;
using System;

namespace QSP
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

        private double GetUVWindHelper(
            double lat, double lon, TableOption para)
        {
            int x = (int)(Math.Floor(lat));
            int y = (int)(Math.Floor(lon));

            // Tricks to prevent interpolation using data that is 
            // out of range of array.
            if (x == 90)
            {
                x = 89;
            }

            if (y == 180)
            {
                y = 179;
            }

            return Interpolate2D.Interpolate(
                x, x + 1, lat,
                y, y + 1, lon,
                GetWindHelper(x, y, para),
                GetWindHelper(x, y + 1, para),
                GetWindHelper(x + 1, y, para),
                GetWindHelper(x + 1, y + 1, para));
        }

        private double GetWindHelper(int lat, int lon, TableOption para)
        {
            if (para == TableOption.U)
            {
                return uTable[lat + 90, lon + 180];
            }
            else
            {
                return vTable[lat + 90, lon + 180];
            }
        }
    }
}
