using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.Tracks.Common
{
    public class TrackAdder<T> where T :ITrack
    {
        protected List<T> allTracks;
        private WaypointList wptList;

        public TrackAdder() : this(RouteFindingCore.WptList) { }

        public TrackAdder(WaypointList wptList)
        {
            this.wptList = wptList;
        }

        public void AddToWaypointList(TrackNodes nodes)
        {

        }

        public void AddToWptList()
        {
            if (allTracks.Count > 0)
            {
                if (allTracks.First() is PacificTrack)
                {
                    wptList.DisableTrack(TrackType.Ausots);
                    wptList.CurrentlyTracked = TrackChangesOption.AddingPacots;
                }
                else
                {
                    wptList.DisableTrack(TrackType.Ausots);
                    wptList.CurrentlyTracked = TrackChangesOption.AddingAusots;
                }

                foreach (var i in allTracks)
                {
                    addTrackToWptList(i);
                }

                wptList.CurrentlyTracked = TrackChangesOption.No;
            }
        }

        private void addTrackToWptList(T item)
        {
            try
            {
                var reader = new TrackReader<T>(item).Read();
                addMainRoute(reader.MainRoute, item);

                if (reader != null)
                {
                    foreach (var i in reader.PairsToAdd)
                    {
                        addPairs(i);
                    }
                }
            }
            catch
            {
                RouteFindingCore.TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution,
                                                             "Failed to process track " + item.Ident + ".",
                                                             (item is PacificTrack) ? TrackType.Pacots : TrackType.Ausots);
            }
        }

        private void addPairs(WptPair item)
        {
            wptList.AddNeighbor(item.IndexFrom, item.IndexTo, new Neighbor("DCT", wptList.Distance(item.IndexFrom, item.IndexTo)));
        }

        private void addMainRoute(Route rte, T trk)
        {
            int indexStart = addFirstWpt(rte.First.Waypoint);
            int indexEnd = addLastWpt(rte.Last.Waypoint);

            wptList.AddNeighbor(indexStart, indexEnd, new Neighbor(trk.AirwayIdent, rte.TotalDistance));
        }

        //returns the index of added wpt in wptList
        private int addFirstWpt(Waypoint wpt)
        {
            int x = wptList.FindByWaypoint(wpt);

            if (x >= 0)
            {
                if (wptList.EdgesToCount(x) == 0)
                {
                    //no other wpt have this wpt as a neighbor, need to find nearby wpt to connect

                    List<int> k = Utilities.NearbyWaypointsInWptList(20, wpt.Lat, wpt.Lon, wptList);

                    foreach (var m in k)
                    {
                        wptList.AddNeighbor(m, x, new Neighbor("DCT", wptList.Distance(x, m)));
                    }
                }
                return x;
            }
            throw new TrackWaypointNotFoundException(string.Format("Waypoint {0} is not found.", wpt.ID));
        }

        private int addLastWpt(Waypoint wpt)
        {
            int x = wptList.FindByWaypoint(wpt);

            if (x >= 0)
            {

                if (wptList.EdgesFromCount(x) == 0)
                {
                    List<int> k = Utilities.NearbyWaypointsInWptList(20, wpt.Lat, wpt.Lon, wptList);

                    foreach (var m in k)
                    {
                        wptList.AddNeighbor(x, m, new Neighbor("DCT", wptList.Distance(x, m)));
                    }
                }
                return x;
            }
            throw new TrackWaypointNotFoundException(string.Format("Waypoint {0} is not found.", wpt.ID));
        }
    }
}
