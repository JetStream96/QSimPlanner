using QSP.RouteFinding.Tracks.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.Routes.TrackInUse
{
    public class TrackInUseCollection
    {
        private static readonly RouteEntry[] EmptyEntry = { };

        public IEnumerable<RouteEntry> Nats { get; private set; }
        public IEnumerable<RouteEntry> Pacots { get; private set; }
        public IEnumerable<RouteEntry> Ausots { get; private set; }

        public TrackInUseCollection()
        {
            Nats = EmptyEntry;
            Pacots = EmptyEntry;
            Ausots = EmptyEntry;
        }

        public IEnumerable<RouteEntry> AllEntries =>
            Nats.Concat(Pacots).Concat(Ausots);
            
        public void Clear()
        {
            Nats = EmptyEntry;
            Pacots = EmptyEntry;
            Ausots = EmptyEntry;
        }

        public void UpdateTracks(
            IEnumerable<TrackNodes> AllNodes, TrackType type)
        {
            var items = AllNodes.Select(i =>
                new RouteEntry(i.MainRoute.Nodes, i.AirwayIdent));

            UpdateList(items, type);
        }

        private void UpdateList(
            IEnumerable<RouteEntry> entries, TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    Nats = entries;
                    break;

                case TrackType.Pacots:
                    Pacots = entries;
                    break;

                case TrackType.Ausots:
                    Ausots = entries;
                    break;

                default:
                    throw new ArgumentException();
            }
        }
    }
}
