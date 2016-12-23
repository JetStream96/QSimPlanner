using QSP.RouteFinding.Data.Interfaces;
using System.Collections.Generic;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.Tracks.Common
{
    // This class is immutable.
    public abstract class Track
    {
        public string Ident { get; private set; }
        public abstract string AirwayIdent { get; }
        public string TimeStart { get; private set; }
        public string TimeEnd { get; private set; }
        public string Remarks { get; private set; }

        public RouteString MainRoute { get; private set; }
        public IReadOnlyList<RouteString> RouteFrom { get; private set; }
        public IReadOnlyList<RouteString> RouteTo { get; private set; }
        public ICoordinate PreferredFirstLatLon { get; private set; }
        public ICoordinate PreferredLastLatLon { get; private set; }

        public Track(
            string Ident,
            string TimeStart,
            string TimeEnd,
            string Remarks,
            RouteString MainRoute,
            IReadOnlyList<RouteString> RouteFrom,
            IReadOnlyList<RouteString> RouteTo,
            ICoordinate PreferredFirstLatLon,
            ICoordinate PreferredLastLatLon)
        {
            this.Ident = Ident;
            this.TimeStart = TimeStart;
            this.TimeEnd = TimeEnd;
            this.Remarks = Remarks;
            this.MainRoute = MainRoute;
            this.RouteFrom = RouteFrom;
            this.RouteTo = RouteTo;
            this.PreferredFirstLatLon = PreferredFirstLatLon;
            this.PreferredLastLatLon = PreferredLastLatLon;
        }
    }
}
