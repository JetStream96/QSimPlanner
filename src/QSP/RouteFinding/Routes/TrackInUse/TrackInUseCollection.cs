using QSP.Common;
using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;

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

        /// <summary>
        /// Creates a deep copy of TrackInUseCollection.
        /// </summary>
        public TrackInUseCollection(TrackInUseCollection other)
        {
            _nats = new List<RouteEntry>(other.Nats);
            _pacots = new List<RouteEntry>(other.Pacots);
            _ausots = new List<RouteEntry>(other.Ausots);
        }

        public IReadOnlyList<RouteEntry>[] AllEntries
        {
            get
            {
                return new IReadOnlyList<RouteEntry>[] 
                {
                    Nats, Pacots, Ausots
                };
            }
        }

        public void UpdateTracks(List<TrackNodes> AllNodes, TrackType type)
        {
            var list = new List<RouteEntry>();

            foreach (var i in AllNodes)
            {
                list.Add(new RouteEntry(i.MainRoute.First.List, i.AirwayIdent));
            }

            UpdateList(list, type);
        }

        private void UpdateList(
            List<RouteEntry> entries, TrackType type)
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
