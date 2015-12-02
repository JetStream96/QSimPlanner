using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using static QSP.LibraryExtension.StringUtilities;
using QSP.AviationTools;
using static QSP.RouteFinding.Constants;
using static QSP.RouteFinding.RouteFindingCore;
using static QSP.MathTools.MathTools;

namespace QSP.RouteFinding.Tracks.Nats
{

    public class NatHandler
    {

        public const string natsUrl = "https://www.notams.faa.gov/common/nat.html?";
        private const string natsWest = "http://qsimplan.somee.com/nats/Westbound.xml";
        private const string natsEast = "http://qsimplan.somee.com/nats/Eastbound.xml";

        private static readonly LatLon CENTER_ATL = new LatLon(55, -45);

        private List<NATsMessage> natMsg;
        private List<NorthAtlanticTrack> NatTrackCollection = new List<NorthAtlanticTrack>();

        public NorthAtlanticTrack GetTrack(char identLetter)
        {
            foreach (var i in NatTrackCollection)
            {
                if (i.Ident == identLetter)
                {
                    return new NorthAtlanticTrack(i);
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
                    Waypoint[] result = new Waypoint[i.WptIndex.Count];
                    for (int j = 0; j < result.Length; j++)
                    {
                        result[j] = WptList.WaypointAt(i.WptIndex[j]);
                    }
                    return result;
                }
            }
            return null;
        }

        public static List<NATsMessage> DownloadFromWeb(string url)
        {
            //the list contains either 1 or 2 item(s)
            List<NATsMessage> result = new List<NATsMessage>();
            string htmlStr = null;

            using (WebClient wc = new WebClient())
            {
                htmlStr = wc.DownloadString(url);
            }

            string time_updated = StringStartEndWith(htmlStr, "Last updated", "</i>", CutStringOptions.PreserveStart);
            string general_info = StringStartEndWith(htmlStr, "The following are active North Atlantic Tracks", "</th>", CutStringOptions.PreserveStart);
            if (htmlStr.IndexOf("EGGXZOZX") >= 0)
            {
                string msg = CutString2(htmlStr, "EGGXZOZX", "</td>", false);
                msg = LibraryExtension.LibraryExtension.ReplaceString(msg, new string[] {
                    "</font>",
                    "<font color=\"#000099\">",
                    new string((char)2,1),new string((char)3,1),new string((char)11,1)}, "");

                result.Add(new NATsMessage(time_updated, general_info, NATsDir.West, msg));
            }

            if (htmlStr.IndexOf("CZQXZQZX") >= 0)
            {
                string msg = CutString2(htmlStr, "CZQXZQZX", "</td>", false);
                msg = LibraryExtension.LibraryExtension.ReplaceString(msg, new string[]{
                    "</font>",
                    "<font color=\"#000099\">",
                     new string((char)2,1),new string((char)3,1),new string((char)11,1) }, "");

                result.Add(new NATsMessage(time_updated, general_info, NATsDir.East, msg));
            }

            return result;

        }

        //Repeated downloads is okay and will not cause any problem.

        public void DownloadNatsMsg()
        {
            natMsg = DownloadFromWeb(natsUrl);


            if (natMsg.Count == 1)
            {
                string downloadAdditional = null;

                if (natMsg[0].Direction == NATsDir.East)
                {
                    downloadAdditional = natsWest;
                }
                else
                {
                    downloadAdditional = natsEast;
                }

                using (WebClient wc = new WebClient())
                {
                    natMsg.Add(new NATsMessage(wc.DownloadString(downloadAdditional)));
                }
            }
        }

        public void AddToWptList()
        {
            NatTrackCollection = new List<NorthAtlanticTrack>();

            foreach (var i in natMsg)
            {
                try
                {
                    NatTrackCollection.AddRange(i.ConvertToTracks());
                }
                catch
                {
                    TrackStatusRecorder.AddEntry(StatusRecorder.Severity.Caution,
                        string.Format("Unable to interpret {0} tracks.", (i.Direction == NATsDir.East) ? "eastbound" : "westbound"), TrackType.Nats);
                }

            }

            //prevent adding the same set of tracks multiple times, if this method is called repeatedly
            WptList.DisableNATs();

            //add wpts
            WptList.TrackChanges = TrackedWptList.TrackChangesOption.AddingNATs;

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

            WptList.TrackChanges = TrackedWptList.TrackChangesOption.No;

        }

        /// <summary>
        /// Add the first waypoint in NAT to WptList, while modifying latLon and FirstTrackWptIndex.
        /// </summary>
        private void addFirstWpt(NorthAtlanticTrack currentTrack, List<LatLon> latLon, ref int FirstTrackWptIndex)
        {
            int x = findInWptListNorthAtlantic(currentTrack.WptIdent[0]);

            if (x >= 0)
            {
                WptNeighbor pt = WptList.ElementAt(x);
                Waypoint q = pt.Waypoint;

                latLon.Add(q.LatLon);
                FirstTrackWptIndex = x;
                currentTrack.WptIndex.Add(x);

                if (WptList.NumberOfNodeFrom(x) > 0)
                {
                    //some other wpt have this wpt as a neighbor
                    return;
                }
                else
                {
                    //no other wpt have this wpt as a neighbor, need to find nearby wpt to connect

                    List<int> k = Common.Utilities.NearbyWaypointsInWptList(20, q.Lat, q.Lon);

                    foreach (int m in k)
                    {
                        WptList.AddNeighbor(m, new Neighbor(x, "DCT", WptList.Distance(x, m)));
                    }

                    return;

                }
            }
            throw new TrackWaypointNotFoundException("Waypoint ident \"" + currentTrack.WptIdent[0] + "\" not found.");
        }

        /// <summary>
        /// Add the last waypoint in NAT to WptList, while modifying latLon and LastTrackWptIndex.
        /// </summary>
        private void addLastWpt(NorthAtlanticTrack currentTrack, List<LatLon> latLon, ref int LastTrackWptIndex)
        {
            int x = findInWptListNorthAtlantic(currentTrack.WptIdent.Last());

            if (x != -1)
            {
                WptNeighbor pt = WptList.ElementAt(x);
                Waypoint q = pt.Waypoint;

                latLon.Add(q.LatLon);
                LastTrackWptIndex = x;
                currentTrack.WptIndex.Add(x);

                if (pt.Neighbors.Count > 0)
                {
                    return;

                }
                else
                {
                    List<int> k = Common.Utilities.NearbyWaypointsInWptList(20, q.Lat, q.Lon);

                    foreach (int m in k)
                    {
                        WptList.AddNeighbor(x, new Neighbor(m, "DCT", WptList.Distance(x, m)));
                    }

                    return;
                }
            }
            throw new TrackWaypointNotFoundException("Waypoint ident \"" + currentTrack.WptIdent.Last() + "\" not found.");
        }

        private void addTracksIntoWptList(NorthAtlanticTrack currentTrack)
        {
            int FirstTrackWptIndex = 0;
            int LastTrackWptIndex = 0;
            //The first and last waypoints in the track. Index in WptList.
            double track_dis = 0;
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
                    latLon.Add(WptList[y].LatLon());
                    currentTrack.WptIndex.Add(y);
                }
                else
                {
                    throw new TrackWaypointNotFoundException("Waypoint ident \"" + currentTrack.WptIdent[i] + "\" not found.");
                }

            }

            for (int i = 0; i <= latLon.Count - 2; i++)
            {
                track_dis += GreatCircleDistance(latLon[i], latLon[i + 1]);
            }

            WptList.AddNeighbor(FirstTrackWptIndex, new Neighbor(LastTrackWptIndex, "NAT" + currentTrack.Ident, track_dis));
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
                Waypoint p = WptList.WaypointAt(i);
                if (WithinNorthAtlanticArea(p.Lat, p.Lon))
                {
                    wptIndex.Add(i);
                }
            }

            if (wptIndex.Count > 1)
            {
                //TODO: possibly change the algorithm to find the wpt closest to the last one
                return chooseWptAtlantic(wptIndex);

                //Throw New Exception("Multiple waypoints with ident """ & ident & """ found in north atlantic region.")
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
                tmp = GreatCircleDistance(CENTER_ATL, WptList.WaypointAt(i).LatLon);
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
            return new Waypoint(NatsLatLonToIdent(s), Convert.ToDouble(s[0] + s[1]), -1 * Convert.ToDouble(s[3] + s[4]));
        }

        public static bool IsNatsLatLonFormat(string s)
        {
            if (s.Length == 5 && s[2] == '/' && char.IsDigit(s[0]) && char.IsDigit(s[1]) && char.IsDigit(s[3]) && char.IsDigit(s[4]))
            {
                return true;
            }
            return false;
        }

        public static bool WithinNorthAtlanticArea(double lat, double lon)
        {
            if (lat > 20.0 && lat < 75.0 && lon < 0.0 && lon > -80.0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
