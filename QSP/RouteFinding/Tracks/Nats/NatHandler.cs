using QSP.AviationTools;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static QSP.LibraryExtension.Strings;
using static QSP.MathTools.Utilities;
using static QSP.RouteFinding.Constants;
using static QSP.RouteFinding.RouteFindingCore;
using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Nats
{

    public class NatHandler
    {
        public const string natsUrl = "https://www.notams.faa.gov/common/nat.html?";
        private const string natsWest = "http://qsimplan.somee.com/nats/Westbound.xml";
        private const string natsEast = "http://qsimplan.somee.com/nats/Eastbound.xml";

        private static readonly LatLon CENTER_ATL = new LatLon(55.0, -45.0);

        private List<IndividualNatsMessage> natMsg;
        private List<NorthAtlanticTrackOld> NatTrackCollection = new List<NorthAtlanticTrackOld>();

        public NorthAtlanticTrackOld GetTrack(char identLetter)
        {
            foreach (var i in NatTrackCollection)
            {
                if (i.Ident == identLetter)
                {
                    return new NorthAtlanticTrackOld(i);
                }
            }
            return null;
        }

        public Waypoint[] GetTrackWaypointArray(char identLetter)
        {
            foreach (var i in NatTrackCollection)
            {
                if (i.Ident == identLetter)
                {
                    var result = new Waypoint[i.WptIndex.Count];
                    for (int j = 0; j < result.Length; j++)
                    {
                        result[j] = WptList[i.WptIndex[j]];
                    }
                    return result;
                }
            }
            return null;
        }

      

        public void AddToWptList()
        {
            NatTrackCollection = new List<NorthAtlanticTrackOld>();

            foreach (var i in natMsg)
            {
                try
                {
                    NatTrackCollection.AddRange(i.ConvertToTracks());
                }
                catch
                {
                    TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution,
                                                 string.Format("Unable to interpret {0} tracks.",
                                                 (i.Direction == NatsDirection.East) ? "eastbound" : "westbound"),
                                                 TrackType.Nats);
                }
            }

            //prevent adding the same set of tracks multiple times, if this method is called repeatedly
            WptList.DisableTrack(TrackType.Nats);

            //add wpts
            WptList.CurrentlyTracked = TrackChangesOption.AddingNats;

            foreach (var t in NatTrackCollection)
            {
                try
                {
                    addTracksIntoWptList(t);
                }
                catch
                {
                    TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution,
                        "Failed to process track " + t.Ident + ".", TrackType.Nats);
                }
            }

            WptList.CurrentlyTracked = TrackChangesOption.No;
        }

        /// <summary>
        /// Add the first waypoint in NAT to WptList, while modifying latLon and FirstTrackWptIndex.
        /// </summary>
        private void addFirstWpt(NorthAtlanticTrackOld currentTrack, List<LatLon> latLon, ref int FirstTrackWptIndex)
        {
            int x = findInWptListNorthAtlantic(currentTrack.WptIdent[0]);

            if (x >= 0)
            {
                var pt = WptList[x];

                latLon.Add(pt.LatLon);
                FirstTrackWptIndex = x;
                currentTrack.WptIndex.Add(x);

                if (WptList.EdgesToCount(x) > 0)
                {
                    //some other wpt have this wpt as a neighbor
                    return;
                }
                else
                {
                    //no other wpt have this wpt as a neighbor, need to find nearby wpt to connect

                    List<int> k = Common.Utilities.NearbyWaypointsInWptList(20, pt.Lat, pt.Lon, WptList);

                    foreach (int m in k)
                    {
                        WptList.AddNeighbor(m, x, new Neighbor("DCT", WptList.Distance(x, m)));
                    }

                    return;

                }
            }
            throw new TrackWaypointNotFoundException("Waypoint ident \"" + currentTrack.WptIdent[0] + "\" not found.");
        }

        /// <summary>
        /// Add the last waypoint in NAT to WptList, while modifying latLon and LastTrackWptIndex.
        /// </summary>
        private void addLastWpt(NorthAtlanticTrackOld currentTrack, List<LatLon> latLon, ref int LastTrackWptIndex)
        {
            int x = findInWptListNorthAtlantic(currentTrack.WptIdent.Last());

            if (x != -1)
            {
                var pt = WptList[x];

                latLon.Add(pt.LatLon);
                LastTrackWptIndex = x;
                currentTrack.WptIndex.Add(x);

                if (WptList.EdgesFromCount(x) > 0)
                {
                    return;
                }
                else
                {
                    List<int> k = Common.Utilities.NearbyWaypointsInWptList(20, pt.Lat, pt.Lon, WptList);

                    foreach (int m in k)
                    {
                        WptList.AddNeighbor(x, m, new Neighbor("DCT", WptList.Distance(x, m)));
                    }

                    return;
                }
            }
            throw new TrackWaypointNotFoundException("Waypoint ident \"" + currentTrack.WptIdent.Last() + "\" not found.");
        }

        private void addTracksIntoWptList(NorthAtlanticTrackOld currentTrack)
        {
            //The first and last waypoints in the track. Index in WptList.
            int FirstTrackWptIndex = 0;
            int LastTrackWptIndex = 0;

            double trackDis = 0;
            var latLon = new List<LatLon>();

            for (int i = 0; i < currentTrack.WptIdent.Count; i++)
            {
                //if the wpt is a coordinate
                if (IsNatsLatLonFormat(currentTrack.WptIdent[i]))
                {
                    currentTrack.WptIdent[i] = NatsLatLonToIdent(currentTrack.WptIdent[i]);
                }

                //first wpt
                if (i == 0)
                {
                    addFirstWpt(currentTrack, latLon, ref FirstTrackWptIndex);
                    continue;
                }

                //last wpt
                if (i == currentTrack.WptIdent.Count - 1)
                {
                    addLastWpt(currentTrack, latLon, ref LastTrackWptIndex);
                    continue;
                }

                int y = findInWptListNorthAtlantic(currentTrack.WptIdent[i]);

                if (y != -1)
                {
                    latLon.Add(WptList[y].LatLon);
                    currentTrack.WptIndex.Add(y);
                }
                else
                {
                    throw new TrackWaypointNotFoundException("Waypoint ident \"" + currentTrack.WptIdent[i] + "\" not found.");
                }

            }

            for (int i = 0; i <= latLon.Count - 2; i++)
            {
                trackDis += GreatCircleDistance(latLon[i], latLon[i + 1]);
            }

            WptList.AddNeighbor(FirstTrackWptIndex, LastTrackWptIndex, new Neighbor("NAT" + currentTrack.Ident, trackDis));
        }

        /// <summary>
        /// Returns the index of waypoint in wptList matching the given ident, and within north Atlantic region.
        /// Return value is -1 if not found.
        /// </summary>
        /// <param name="ident"></param>
        private static int findInWptListNorthAtlantic(string ident)
        {
            var findByID = WptList.FindAllByID(ident);

            if (findByID == null)
            {
                return -1;
            }

            List<int> wptIndex = new List<int>();

            foreach (int i in findByID)
            {
                Waypoint p = WptList[i];
                if (WithinNorthAtlanticArea(p.Lat, p.Lon))
                {
                    wptIndex.Add(i);
                }
            }

            if (wptIndex.Count > 1)
            {
                return chooseWptAtlantic(wptIndex);
            }
            else if (wptIndex.Count == 1)
            {
                return wptIndex[0];
            }
            return -1;
        }

        private static int chooseWptAtlantic(List<int> item)
        {
            int result = 0;
            double minDisFromCenter = MAX_DIS;
            double tmp = 0;

            foreach (int i in item)
            {
                tmp = GreatCircleDistance(CENTER_ATL, WptList[i].LatLon);
                if (tmp < minDisFromCenter)
                {
                    result = i;
                    minDisFromCenter = tmp;
                }
            }
            return result;
        }

        /// <summary>
        /// Sample input : "54/20". Sample output : "5420N"
        /// </summary>
        public static string NatsLatLonToIdent(string s)
        {
            return new string(new char[] { s[0], s[1], s[3], s[4], 'N' });
        }

        public static Waypoint NatsLatLonToWaypoint(string s)
        {
            return new Waypoint(NatsLatLonToIdent(s), Convert.ToDouble(s[0] + s[1]), -( Convert.ToDouble(s[3] + s[4])));
        }

        public static bool IsNatsLatLonFormat(string s)
        {
            return (s.Length == 5 &&
                    s[2] == '/' &&
                    char.IsDigit(s[0]) &&
                    char.IsDigit(s[1]) &&
                    char.IsDigit(s[3]) &&
                    char.IsDigit(s[4]));
        }

        public static bool WithinNorthAtlanticArea(double lat, double lon)
        {
            return (lat > 20.0 && lat < 75.0 &&
                    lon < 0.0 && lon > -80.0);
        }
    }

}
