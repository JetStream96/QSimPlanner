using QSP.RouteFinding.Routes;
using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Common
{
    public class TrackNodes
    {
        public string Ident { get; private set; }
        public string AirwayIdent { get; private set; }
        public IEnumerable<WptPair> ConnectionRoutes { get; private set; }

        private Route _mainRoute;
        public Route MainRoute { get { return new Route(_mainRoute); } }

        public TrackNodes(
            string Ident,
            string AirwayIdent,
            Route mainRoute,
            IEnumerable<WptPair> routeFromTo)
        {
            this.Ident = Ident;
            this.AirwayIdent = AirwayIdent;
            _mainRoute = mainRoute;
            ConnectionRoutes = routeFromTo;
        }
    }
}
