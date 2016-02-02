using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.Lists;
using static QSP.LibraryExtension.StringParser.Utilities;

namespace QSP.RouteFinding.Tracks.Common.TDM.Parser
{
    public class ConnectionRouteInterpreter
    {
        private string[] mainRoute;
        private ReadOnlyCollection<string> connectRoutes;
        private AirportManager airportList;

        public ConnectionRouteInterpreter(
                            string[] mainRoute, 
                            ReadOnlyCollection<string> connectRoutes, 
                            AirportManager airportList)
        {
            this.mainRoute = mainRoute;
            this.connectRoutes = connectRoutes;
            this.airportList = airportList;
        }

        public ConnectionRoutes Convert()
        {
            var result = new ConnectionRoutes();

            foreach (var i in connectRoutes)
            {
                var rte = i.Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);

                if (rte != null && rte.Length > 1)
                {
                    if (rte[0] == mainRoute.Last())
                    {
                        //this route is routeTo
                        if (airportList.Find(rte.Last()) != null)
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
                        if (airportList.Find(rte[0]) != null)
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
