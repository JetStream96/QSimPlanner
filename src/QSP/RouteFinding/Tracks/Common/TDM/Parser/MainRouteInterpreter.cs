using System;

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
            return route.Split(new char[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
