using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Coordinates.Formatter;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.Lists;
namespace QSP.RouteFinding.Tracks.Common.TDM.Parser
{
    public static class ConnectionRouteInterpreter
    {
        private static readonly char[] Delimeters = new char[] { ' ', '\n', '\r', '\t' };

        public static ConnectionRoutes Convert(
            IReadOnlyList<string> mainRoute,
            IEnumerable<string> connectRoutes,
            AirportManager airportList)
        {
            var from = new List<RouteString>();
            var to = new List<RouteString>();

            foreach (var i in connectRoutes)
            {
                var route = i
                    .Split(Delimeters, StringSplitOptions.RemoveEmptyEntries)
                    .Select(TryTransformCoordinate)
                    .ToList();

                // Invalid route, ignore.
                if (route.Count <= 1) continue;

                if (route[0] == mainRoute.Last())
                {
                    // This route is routeTo
                    if (airportList[route.Last()] != null)
                    {
                        to.Add(route.Take(route.Count - 1).ToRouteString());
                    }
                    else
                    {
                        to.Add(route.ToRouteString());
                    }
                }
                else if (route.Last() == mainRoute[0])
                {
                    // This route is routeFrom
                    if (airportList[route[0]] != null)
                    {
                        from.Add(route.Skip(1).ToRouteString());
                    }
                    else
                    {
                        from.Add(route.ToRouteString());
                    }
                }
            }

            return new ConnectionRoutes { RouteFrom = from, RouteTo = to };
        }

        public class ConnectionRoutes
        {
            public IReadOnlyList<RouteString> RouteFrom, RouteTo;
        }
    }
}
