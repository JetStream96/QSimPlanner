using System;
using static QSP.LibraryExtension.StringParser.Utilities;

namespace QSP.RouteFinding.Tracks.Common.Parser
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
            var routeSplit = route.Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);
            throw new NotImplementedException();
        }
    }
}
