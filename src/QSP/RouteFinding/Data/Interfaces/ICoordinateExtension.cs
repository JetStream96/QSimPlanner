using QSP.LibraryExtension;
using QSP.MathTools;
using System.Collections.Generic;

namespace QSP.RouteFinding.Data.Interfaces
{
    public static class ICoordinateExtension
    {
        public static double Distance<T>(this T x, T y) where T : ICoordinate
        {
            return Distance(x, y.Lat, y.Lon);
        }

        public static double Distance<T>(this T x, double lat, double lon) 
            where T : ICoordinate
        {
            return GCDis.Distance(x.Lat, x.Lon, lat, lon);
        }

        public static double TotalDistance<T>(this IEnumerable<T> source)
            where T : ICoordinate
        {
            double dis = 0.0;
            var enumerator = source.GetEnumerator();
            bool started = false;

            T current = default(T);

            while (enumerator.MoveNext())
            {
                if (started)
                {
                    var prev = current;
                    current = enumerator.Current;
                    dis += Distance(current, prev);
                }
                else
                {
                    current = enumerator.Current;
                    started = true;
                }
            }

            return dis;
        }

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static T GetClosest<T>(
            this IEnumerable<T> items, double Lat, double Lon)
            where T : ICoordinate
        {
            return items.MinBy(i => i.Distance(Lat, Lon));
        }
    }
}
