using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using System;
using System.Collections.Generic;
using static QSP.MathTools.GCDis;

namespace QSP.RouteFinding.Tracks.Common
{
    public static class Utilities
    {
        public static int ChooseSubsequentWpt(double prevLat,
                                              double prevLon,
                                              List<int> candidates,
                                              WaypointList wptList)
        {
            if (candidates == null || candidates.Count == 0)
            {
                throw new ArgumentException("List cannot be null or empty.");
            }

            double minDis = double.MaxValue;
            int minIndex = 0;
            double dis = 0.0;

            for (int i = 0; i < candidates.Count; i++)
            {
                var wpt = wptList[candidates[i]];
                dis = Distance(prevLat, prevLon, wpt.Lat, wpt.Lon);

                if (dis < minDis)
                {
                    minIndex = i;
                    minDis = dis;
                }
            }
            return candidates[minIndex];
        }

        public static void ConvertLatLonFormat(string[] item)
        {
            for (int i = 0; i < item.Length; i++)
            {
                LatLon result;

                if (Format7Letter.TryReadFrom7LetterFormat(item[i], out result))
                {
                    item[i] = result.To5LetterFormat();
                }
            }
        }

        /// <summary>
        /// Returns the indices of waypoints which are closest to a specific lat/lon. 
        /// </summary>
        public static List<int> NearbyWaypointsInWptList(int count,
                                                         double lat,
                                                         double lon,
                                                         WaypointList wptList)
        {
            var x = WaypointAirwayConnector.FindAirwayConnection(lat, lon, wptList);
            var result = new List<int>(x.Count);

            foreach (var i in x)
            {
                result.Add(i.Index);
            }
            return result;
        }
    }
}
