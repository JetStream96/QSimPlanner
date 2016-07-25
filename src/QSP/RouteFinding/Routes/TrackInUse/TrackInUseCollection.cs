using QSP.Common;
using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.Routes.TrackInUse
{
    public class TrackInUseCollection
    {
        private List<RouteEntry> _nats;
        private List<RouteEntry> _pacots;
        private List<RouteEntry> _ausots;

        public IReadOnlyList<RouteEntry> Nats
        {
            get
            {
                return _nats;
            }
        }

        public IReadOnlyList<RouteEntry> Pacots
        {
            get
            {
                return _pacots;
            }
        }

        public IReadOnlyList<RouteEntry> Ausots
        {
            get
            {
                return _ausots;
            }
        }

        public TrackInUseCollection()
        {
            _nats = new List<RouteEntry>();
            _pacots = new List<RouteEntry>();
            _ausots = new List<RouteEntry>();
        }
        
        public IEnumerable<RouteEntry> AllEntries
        {
            get
            {
                return Nats.Union(Pacots).Union(Ausots);
            }
        }

        public void UpdateTracks(List<TrackNodes> AllNodes, TrackType type)
        {
            var list = new List<RouteEntry>();

            foreach (var i in AllNodes)
            {
                list.Add(new RouteEntry(i.MainRoute.Nodes, i.AirwayIdent));
            }

            UpdateList(list, type);
        }

        private void UpdateList(List<RouteEntry> entries, TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    _nats = entries;
                    break;

                case TrackType.Pacots:
                    _pacots = entries;
                    break;

                case TrackType.Ausots:
                    _ausots = entries;
                    break;

                default:
                    throw new EnumNotSupportedException();
            }
        }
    }
}
