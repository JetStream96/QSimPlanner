using System;
using static QSP.LibraryExtension.StringParser.Utilities;

namespace QSP.RouteFinding.Tracks.Common.TDM.Parser
{
    public class MainRouteInterpreter
    {
        private string route;

        public MainRouteInterpreter(string route)
        {
            this.route = route;
        }

        public string[] Convert()
        {
            return route.Split(DelimiterWords,StringSplitOptions.RemoveEmptyEntries);          
        }
    }
}
