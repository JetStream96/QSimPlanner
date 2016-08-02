using QSP.RouteFinding.Tracks.Common;
using RouteString = System.Collections.Generic.IReadOnlyList<string>;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NorthAtlanticTrack : Track
    {
        private static readonly string[][] EmptyCollection = new string[0][];

        public NatsDirection Direction { get; private set; }

        public sealed override string AirwayIdent
        {
            get { return "NAT" + Ident; }
        }

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
                   EmptyCollection,
                   EmptyCollection,
                   Constants.CenterAtlantic,
                   Constants.CenterAtlantic)
        {
            this.Direction = Direction;
        }
    }
}
