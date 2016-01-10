using QSP.RouteFinding.Routes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.Tracks.Common
{
    public class TrackNodes
    {
        private List<WptPair> _routeFromTo;
        private Route _mainRoute;
        
        public string AirwayIdent { get; private set; }

        public Route MainRoute
        {
            get
            {
                return new Route(_mainRoute);
            }
        }

        public ReadOnlyCollection<WptPair> PairsToAdd
        {
            get { return _routeFromTo.AsReadOnly(); }
        }

        public TrackNodes(string AirwayIdent, Route mainRoute, List<WptPair> routeFromTo)
        {
            this.AirwayIdent = AirwayIdent;
            _mainRoute = mainRoute;
            _routeFromTo = routeFromTo;
        }
    }
}
