using QSP.Utilities;
using System;
using System.Collections.Generic;
using static QSP.MathTools.GCDis;

namespace QSP.RouteFinding.Data.Interfaces
{
    public static class ExtensionMethods
    {
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
                double dis = Distance(i.Lat, i.Lon, Lat, Lon);

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
