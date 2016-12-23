using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NorthAtlanticTrack : Track
    {
        private static readonly RouteString[] EmpRouteStrings = new RouteString[0];

        public NatsDirection Direction { get; private set; }

        public sealed override string AirwayIdent => "NAT" + Ident;

        public NorthAtlanticTrack(
            NatsDirection Direction,
            string Ident,
            string TimeStart,
            string TimeEnd,
            string Remarks,
            RouteString MainRoute)
            : base(Ident,
                   TimeStart,
                   TimeEnd,
                   Remarks,
                   MainRoute,
                   EmpRouteStrings,
                   EmpRouteStrings,
                   Constants.CenterAtlantic,
                   Constants.CenterAtlantic)
        {
            this.Direction = Direction;
        }
    }
}
