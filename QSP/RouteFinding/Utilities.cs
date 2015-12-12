using System;
using System.Collections.Generic;
using QSP.RouteFinding.Containers;
using static QSP.RouteFinding.RouteFinder;
using static QSP.RouteFinding.RouteFindingCore;
using static QSP.MathTools.MathTools;
using QSP.AviationTools;

namespace QSP.RouteFinding
{

    public static class Utilities
    {

        private const double MAX_LEG_DIS = 500.0;

        /// <summary>
        /// Determines whether the input string is a valid rwy identifier.
        /// </summary>
        public static bool IsRwyIdent(string str)
        {
            int i = 0;

            if (str.Length == 2)
            {
                if (int.TryParse(str, out i))
                {
                    if (i > 0 && i <= 36)
                    {
                        return true;
                    }
                }
            }
            else if (str.Length == 3)
            {
                if (str[2] == 'L' || str[2] == 'R' || str[2] == 'C')
                {
                    if (int.TryParse(str.Substring(0, 2), out i))
                    {
                        if (i > 0 & i <= 36)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }

        public static int HasCorrds(string fixType)
        {
            string[] correct_fix_type = { "IF", "DF", "TF", "FD", "CF" };
            //, "AF", "RF", "CD", "FA", "FC", "FM", "VD", "PI", "HF", "HA", "HM"}
            //this lists all kinds of fixes with coordinates located in t(2) and t(3)

            foreach (var i in correct_fix_type)
            {
                if (i == fixType)
                {
                    return 0;
                }
            }
            return -1;
        }

        public static double GetTotalDistance(List<LatLon> latLons)
        {
            //in nm
            if (latLons.Count < 2)
            {
                return 0;
            }
            double dis = 0;

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
                dis += wpts[i].LatLon.Distance(wpts[i + 1].LatLon);
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

        public static List<Neighbor> sidStarToAirwayConnection(string sidStarName, LatLon latLon, double initDis)
        {
            const double SEARCH_RANGE_INCR = 20.0;
            const double MAX_SEARCH_RANGE = 1000.0;
            const int TARGET_NUM = 30;

            double searchRange = 0;
            List<Neighbor> wptsOnAirway = new List<Neighbor>();

            while (searchRange <= MAX_SEARCH_RANGE)
            {
                wptsOnAirway.Clear();
                searchRange += SEARCH_RANGE_INCR;

                var searchResult = WptFinder.Find(latLon.Lat, latLon.Lon, searchRange);
                double dctDis = 0;

                foreach (int i in searchResult)
                {
                    if (WptList[i].Neighbors.Count > 0)
                    {
                        dctDis = WptList.LatLonAt(i).Distance(latLon);

                        if (dctDis <= searchRange)
                        {
                            wptsOnAirway.Add(new Neighbor(i, sidStarName, dctDis + initDis));
                        }
                    }
                }

                if (wptsOnAirway.Count >= TARGET_NUM)
                {
                    return wptsOnAirway;
                }
            }
            return wptsOnAirway;
        }              
        
    }

}

