using QSP.MathTools;
using QSP.Utilities;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.Data.Interfaces
{
    public static class ICoordinateExtension
    {
        public static double Distance<T>(this T x, T y) where T : ICoordinate
        {
            return GCDis.Distance(x.Lat, x.Lon, y.Lat, y.Lon);
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
        
        /// <exception cref="ArgumentException"></exception>
        public static T GetClosest<T>(
            this IEnumerable<T> items, double Lat, double Lon)
            where T : ICoordinate
        {
            int count = 0;
            T closest = default(T);
            double minDis = double.MaxValue;

            foreach (var i in items)
            {
                count++;
                double dis = GCDis.Distance(i.Lat, i.Lon, Lat, Lon);

                if (dis < minDis)
                {
                    minDis = dis;
                    closest = i;
                }
            }

            ConditionChecker.Ensure<ArgumentException>(count > 0);
            return closest;
        }
    }
}
