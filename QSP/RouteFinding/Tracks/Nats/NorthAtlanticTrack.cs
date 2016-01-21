using System.Collections.Generic;
using System.Collections.ObjectModel;
using QSP.AviationTools;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NorthAtlanticTrack : Track
    {
        private static readonly ReadOnlyCollection<string[]> EmptyCollection = new List<string[]>().AsReadOnly();

        public NatsDirection Direction { get; private set; }

        public sealed override string AirwayIdent
        {
            get
            {
                return "NAT" + Ident;
            }
        }

        public NorthAtlanticTrack(NatsDirection Direction,
                                  string Ident,
                                  string TimeStart,
                                  string TimeEnd,
                                  string Remarks,
                                  ReadOnlyCollection<string> MainRoute,
                                  LatLon PreferredFirstLatLon)

            : base(Ident, TimeStart, TimeEnd, Remarks, MainRoute, EmptyCollection, EmptyCollection, PreferredFirstLatLon)
        {
            this.Direction = Direction;
        }
    }
}
