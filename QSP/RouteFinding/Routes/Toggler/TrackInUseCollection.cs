using QSP.Core;
using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.Routes.Toggler
{
    public class TrackInUseCollection
    {
        private List<RouteEntry> _nats;
        private List<RouteEntry> _pacots;
        private List<RouteEntry> _ausots;

        public ReadOnlyCollection<RouteEntry> Nats
        {
            get
            {
                return _nats.AsReadOnly();
            }
        }

        public ReadOnlyCollection<RouteEntry> Pacots
        {
            get
            {
                return _pacots.AsReadOnly();
            }
        }
    
        public ReadOnlyCollection<RouteEntry> Ausots
        {
            get
            {
                return _ausots.AsReadOnly();
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

        public ReadOnlyCollection<RouteEntry>[] AllEntries
        {
            get
            {
                return new ReadOnlyCollection<RouteEntry>[3] { Nats, Pacots, Ausots };
            }
        }
        
        public void UpdateTracks(List<TrackNodes> AllNodes, TrackType type)
        {
            var list = new List<RouteEntry>();

            foreach (var i in AllNodes)
            {
                list.Add(new RouteEntry(i.MainRoute.FirstNode.List, i.AirwayIdent));
            }

            var listToUpdate = selectList(type);
            listToUpdate = list;
        }

        private List<RouteEntry> selectList(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    return _nats;

                case TrackType.Pacots:
                    return _pacots;

                case TrackType.Ausots:
                    return _ausots;

                default:
                    throw new EnumNotSupportedException();
            }
        }
    }
}
