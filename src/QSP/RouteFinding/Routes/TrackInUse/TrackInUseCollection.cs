using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.Routes.TrackInUse
{
    public class TrackInUseCollection
    {
        private static readonly RouteEntry[] EmptyEntry = { };
        private IEnumerable<RouteEntry>[] _entries;

        public TrackInUseCollection()
        {
            _entries = GetNewArray();
        }

        public IEnumerable<RouteEntry> Entries(TrackType type) => _entries[(int)type];

        public IEnumerable<RouteEntry> AllEntries => _entries.SelectMany(i => i);

        private static IEnumerable<RouteEntry>[] GetNewArray()
        {
            var array = new IEnumerable<RouteEntry>[3];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = EmptyEntry;
            }
            return array;
        }

        public void Clear()
        {
            _entries = GetNewArray();
        }

        public void UpdateTracks(IEnumerable<TrackNodes> AllNodes, TrackType type)
        {
            var items = AllNodes.Select(i => new RouteEntry(i.MainRoute.Nodes, i.AirwayIdent));
            UpdateList(items, type);
        }

        private void UpdateList(IEnumerable<RouteEntry> entries, TrackType type)
        {
            _entries[(int)type] = entries;
        }
    }
}
