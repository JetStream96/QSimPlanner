using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;

using RouteString = System.Collections.Generic.IReadOnlyList<string>;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusTrack : Track
    {
        public override string AirwayIdent { get { return "AUSOT" + Ident; } }

        public AusTrack(
            string Ident,
            string TimeStart,
            string TimeEnd,
            string Remarks,
            RouteString MainRoute,
            IReadOnlyList<RouteString> RouteFrom,
            IReadOnlyList<RouteString> RouteTo,
            ICoordinate PreferredFirstLatLon,
            ICoordinate PreferredLastLatLon)
            : base(Ident, TimeStart, TimeEnd, Remarks, MainRoute, RouteFrom,
                  RouteTo, PreferredFirstLatLon, PreferredFirstLatLon)
        { }
    }
}
