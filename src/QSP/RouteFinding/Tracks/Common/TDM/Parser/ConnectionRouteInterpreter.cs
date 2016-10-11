using QSP.RouteFinding.Airports;
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
        public static ConnectionRoutes Convert(
            string[] mainRoute,
            IEnumerable<string> connectRoutes, 
            AirportManager airportList)
        {
            var result = new ConnectionRoutes();

            foreach (var i in connectRoutes)
            {
                var rte = i
                    .Split(new char[] { ' ', '\n', '\r', '\t' }, 
                           StringSplitOptions.RemoveEmptyEntries)
                    .Select(TryTransformCoordinate)
                    .ToArray();

                if (rte != null && rte.Length > 1)
                {
                    if (rte[0] == mainRoute.Last())
                    {
                        // This route is routeTo
                        if (airportList[rte.Last()] != null)
                        {
                            result.RouteTo.Add(rte.SubArray(0, rte.Length - 1));
                        }
                        else
                        {
                            result.RouteTo.Add(rte);
                        }
                    }
                    else if (rte.Last() == mainRoute[0])
                    {
                        if (airportList[rte[0]] != null)
                        {
                            result.RouteFrom.Add(rte.SubArray(1, rte.Length - 1));
                        }
                        else
                        {
                            result.RouteFrom.Add(rte);
                        }
                    }
                }
            }

            return result;
        }

        public class ConnectionRoutes
        {
            public List<string[]> RouteFrom = new List<string[]>();
            public List<string[]> RouteTo = new List<string[]>();
        }
    }
}
