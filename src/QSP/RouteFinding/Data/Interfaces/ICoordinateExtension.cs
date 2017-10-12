using CommonLibrary.LibraryExtension;
using QSP.AviationTools.Coordinates;
using QSP.MathTools;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace QSP.RouteFinding.Data.Interfaces
{
    public static class ICoordinateExtension
    {
        public static bool LatLonEquals(this ICoordinate x, ICoordinate y, double delta = 0.0)
        {
            return Abs(x.Lat - y.Lat) <= delta && Abs(x.Lon - y.Lon) <= delta;
        }

        public static double Distance(this ICoordinate x, ICoordinate y)
        {
            return Distance(x, y.Lat, y.Lon);
        }

        public static double Distance(this ICoordinate x, double lat, double lon)
        {
            return GCDis.Distance(x.Lat, x.Lon, lat, lon);
        }

        public static double TotalDistance(this IEnumerable<ICoordinate> source)
        {
            double distance = 0.0;
            ICoordinate last = null;

            foreach (var i in source)
            {
                if (last != null) distance += last.Distance(i);
                last = i;
            }

            return distance;
        }

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static T GetClosest<T>(this IEnumerable<T> items, ICoordinate coordinate)
            where T : ICoordinate
        {
            return items.GetClosest(coordinate.Lat, coordinate.Lon);
        }

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static T GetClosest<T>(this IEnumerable<T> items, double lat, double lon)
            where T : ICoordinate
        {
            return items.MinBy(i => i.Distance(lat, lon));
        }

        /// <exception cref="ArgumentException"></exception>
        public static Route ToRoute(this IEnumerable<ICoordinate> coordinates)
        {
            var items = coordinates.ToList();
            if (items.Count < 2) throw new ArgumentException();

            var result = new Route();

            foreach (var i in items)
            {
                double lat = i.Lat;
                double lon = i.Lon;

                string latLonTxt =
                    Format5Letter.To5LetterFormat(lat, lon) ??
                    FormatDecimal.ToDecimalFormat(lat, lon);

                var wpt = new Waypoint(latLonTxt, lat, lon);
                result.AddLastWaypoint(wpt, "DCT");
            }

            return result;
        }
    }
}
