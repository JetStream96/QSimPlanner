using System;
using System.Collections.Generic;
using QSP.RouteFinding.Containers;
using static QSP.RouteFinding.RouteFinder;
using static QSP.RouteFinding.RouteFindingCore;
using static QSP.MathTools.MathTools;
using QSP.AviationTools;
using QSP.RouteFinding.AirwayStructure;
using static QSP.RouteFinding.Constants;

namespace QSP.RouteFinding
{

    public static class Utilities
    {
        private static string[] correctFixType = { "IF", "DF", "TF", "FD", "CF" };
        //, "AF", "RF", "CD", "FA", "FC", "FM", "VD", "PI", "HF", "HA", "HM"}
        //this lists all kinds of fixes with coordinates located in t(2) and t(3)

        /// <summary>
        /// Determines whether the input string is a valid rwy identifier.
        /// </summary>
        public static bool IsRwyIdent(string str)
        {
            int i = 0;

            if (str.Length == 2 && int.TryParse(str, out i) && (i > 0 && i <= 36))
            {
                return true;
            }
            else if (str.Length == 3 && (str[2] == 'L' || str[2] == 'R' || str[2] == 'C') && int.TryParse(str.Substring(0, 2), out i)
                      && (i > 0 && i <= 36))
            {
                return true;
            }
            return false;
        }

        public static bool HasCorrds(string fixType)
        {
            foreach (var i in correctFixType)
            {
                if (i == fixType)
                {
                    return true;
                }
            }
            return false;
        }

        public static double GetTotalDistance(List<LatLon> latLons)
        {
            //in nm
            if (latLons.Count < 2)
            {
                return 0.0;
            }
            double dis = 0.0;

            for (int i = 0; i < latLons.Count - 1; i++)
            {
                dis += GreatCircleDistance(latLons[i], latLons[i + 1]);
            }
            return dis;
        }

        public static double GetTotalDistance(List<Waypoint> wpts)
        {
            //in nm
            if (wpts.Count < 2)
            {
                return 0.0;
            }
            double dis = 0.0;

            for (int i = 0; i < wpts.Count - 1; i++)
            {
                dis += GreatCircleDistance(wpts[i].Lat, wpts[i].Lon,
                                           wpts[i + 1].Lat, wpts[i + 1].Lon);
            }
            return dis;
        }

        public static bool HasRwySpecificPart(string[] allLines, string sidOrStarNameNoTrans)
        {
            foreach (var i in allLines)
            {
                string[] t = i.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (t.Length >= 3 && t[1] == sidOrStarNameNoTrans && IsRwyIdent(t[2]))
                {
                    return true;
                }
            }
            return false;
        }

        public static Waypoint FindWpt(string ID, Waypoint lastWpt)
        {
            var matches = WptList.FindAllByID(ID);

            if (matches != null)
            {
                foreach (int i in matches)
                {
                    var wpt = WptList[i];

                    if (lastWpt.LatLon.Distance(wpt.LatLon) <= MAX_LEG_DIS)
                    {
                        return wpt;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a list of waypoints which is near the given Lat/Lon, and are connected to at least one other waypoint.
        /// </summary>
        public static List<IndexDistancePair> FindAirwayConnection(LatLon LatLon, WaypointList wptList)
        {
            return FindAirwayConnection(LatLon.Lat, LatLon.Lon, wptList);
        }

        /// <summary>
        /// Finds a list of waypoints which is near the given Lat/Lon, and are connected to at least one other waypoint.
        /// </summary>
        public static List<IndexDistancePair> FindAirwayConnection(double Lat, double Lon, WaypointList wptList)
        {
            const double SEARCH_RANGE_INCR = 20.0;
            const double MAX_SEARCH_RANGE = MAX_LEG_DIS;
            const int TARGET_NUM = 30;

            double searchRange = 0;
            var result = new List<IndexDistancePair>();

            while (searchRange <= MAX_SEARCH_RANGE)
            {
                result.Clear();
                searchRange += SEARCH_RANGE_INCR;

                var searchResult = wptList.Find(Lat, Lon, searchRange);
                double dctDis = 0;

                foreach (var item in searchResult)
                {
                    int i = item.Index;

                    if (wptList.EdgesFromCount(i) > 0)
                    {
                        dctDis = GreatCircleDistance(wptList[i].Lat, wptList[i].Lon, Lat, Lon);

                        if (dctDis <= searchRange)
                        {
                            result.Add(new IndexDistancePair(i, dctDis));
                        }
                    }
                }

                if (result.Count >= TARGET_NUM)
                {
                    return result;
                }
            }
            return result;
        }

    }

}

