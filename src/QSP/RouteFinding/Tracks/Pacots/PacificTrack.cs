using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacificTrack : Track
    {
        public PacotDirection Direction { get; private set; }

        public sealed override string AirwayIdent => "PACOT" + Ident;

        public PacificTrack(
            PacotDirection Direction,
            string Ident,
            string TimeStart,
            string TimeEnd,
            string Remarks,
            RouteString MainRoute,
            IReadOnlyList<RouteString> RouteFrom,
            IReadOnlyList<RouteString> RouteTo,
            ICoordinate PreferredFirstLatLon,
            ICoordinate PreferredLastLatLon)
             : base(
                   Ident,
                   TimeStart,
                   TimeEnd,
                   Remarks,
                   MainRoute,
                   RouteFrom,
                   RouteTo,
                   PreferredFirstLatLon,
                   PreferredLastLatLon)
        {
            this.Direction = Direction;
        }
    }
}
