using System.Collections.Generic;
using System.Collections.ObjectModel;
using QSP.AviationTools;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NorthAtlanticTrack : ITrack
    {
        private static readonly ReadOnlyCollection<string[]> EmptyCollection = new List<string[]>().AsReadOnly();

        #region Properties

        public NatsDirection Direction { get; private set; }
        public string Ident { get; private set; }
        public ReadOnlyCollection<string> MainRoute { get; private set; }
        public LatLon PreferredFirstLatLon { get; private set; }
        public string Remarks { get; private set; }
        public string TimeEnd { get; private set; }
        public string TimeStart { get; private set; }

        public ReadOnlyCollection<string[]> RouteFrom
        {
            get
            {
                return EmptyCollection;
            }
        }

        public ReadOnlyCollection<string[]> RouteTo
        {
            get
            {
                return EmptyCollection;
            }
        }

        public string AirwayIdent
        {
            get
            {
                return "NAT" + Ident;
            }
        }

        #endregion

        public NorthAtlanticTrack(NatsDirection Direction,
                                  string Ident,
                                  ReadOnlyCollection<string> MainRoute,
                                  LatLon PreferredFirstLatLon,
                                  string Remarks,
                                  string TimeStart,
                                  string TimeEnd)
        {
            this.Direction = Direction;
            this.Ident = Ident;
            this.MainRoute = MainRoute;
            this.PreferredFirstLatLon = PreferredFirstLatLon;
            this.Remarks = Remarks;
            this.TimeStart = TimeStart;
            this.TimeEnd = TimeEnd;
        }

    }
}
