using System;
using QSP.MathTools.Interpolation;
using static QSP.LibraryExtension.Types;

namespace QSP.WindAloft
{
    public class TableItem
    {
        /// <summary>
        /// The index (i,j) corresponds to value at (lat=i+90, lon=j+180).
        /// </summary>
        public double[,] Values { get; set; }

        public double ValueAt(double lat, double lon)
        {
            var x = (int)(Math.Floor(lat));
            var y = (int)(Math.Floor(lon));

            // Prevent interpolation using data that is out of range of array.
            if (x == 90) x = 89;
            if (y == 180) y = 179;

            return Interpolate2D.Interpolate(
                Arr<double>(x, x + 1),
                Arr<double>(y, y + 1),
                Arr(
                    Arr(Helper(x, y), Helper(x, y + 1)),
                    Arr(Helper(x + 1, y), Helper(x + 1, y + 1))
                ),
                x, y);
        }

        private double Helper(int lat, int lon) => Values[lat + 90, lon + 180];
    }
}
