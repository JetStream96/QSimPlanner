using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Common
{
    public class TrackAdder
    {
        private WaypointList wptList;
        private WaypointListEditor editor;
        private StatusRecorder recorder;
        private TrackType type;

        public TrackAdder(WaypointList wptList, 
                          WaypointListEditor editor, 
                          StatusRecorder recorder, 
                          TrackType type)
        {
            this.wptList = wptList;
            this.editor = editor;
            this.recorder = recorder;
            this.type = type;
        }

        public void AddToWaypointList<T>(T nodes) where T : IEnumerable<TrackNodes>
        {
            foreach (var i in nodes)
            {
                addTrackToWptList(i);
            }
        }

        private void addTrackToWptList(TrackNodes item)
        {
            try
            {
                addMainRoute(item);

                foreach (var i in item.PairsToAdd)
                {
                    addPairs(i);
                }
            }
            catch
            {
                recorder.AddEntry(StatusRecorder.Severity.Caution,
                                  "Failed to process track " + item.Ident + ".",
                                  type);
            }
        }

        private void addPairs(WptPair item)
        {
            editor.AddNeighbor(
                item.IndexFrom, 
                item.IndexTo, 
                new Neighbor("DCT", wptList.Distance(item.IndexFrom, item.IndexTo)));
        }

        private void addMainRoute(TrackNodes nodes)
        {
            var rte = nodes.MainRoute;

            int indexStart = addFirstWpt(rte.First.Waypoint);
            int indexEnd = addLastWpt(rte.Last.Waypoint);

            editor.AddNeighbor(indexStart,
                               indexEnd, 
                               new Neighbor(nodes.AirwayIdent, rte.TotalDistance));
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
                        editor.AddNeighbor(m, x, new Neighbor("DCT", wptList.Distance(x, m)));
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
                        editor.AddNeighbor(x, m, new Neighbor("DCT", wptList.Distance(x, m)));
                    }
                }
                return x;
            }
            throw new TrackWaypointNotFoundException(string.Format("Waypoint {0} is not found.", wpt.ID));
        }
    }
}
