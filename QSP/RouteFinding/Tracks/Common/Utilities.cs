using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.Arrays;
using static QSP.RouteFinding.Constants;
using static QSP.MathTools.Utilities;

namespace QSP.RouteFinding.Tracks.Common
{
    public static class Utilities
    {
        public static int ChooseSubsequentWpt(double prevLat, double prevLon, List<int> candidates,WaypointList wptList)
        {
            if (candidates == null || candidates.Count == 0)
            {
                throw new ArgumentException("List cannot be nothing or empty.");
            }

            double minDis = MAX_DIS;
            int minIndex = 0;
            double dis = 0.0;

            for (int i = 0; i < candidates.Count; i++)
            {
                var wpt = wptList[candidates[i]];
                dis = GreatCircleDistance(prevLat, prevLon, wpt.Lat, wpt.Lon);

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
        public static List<int> NearbyWaypointsInWptList(int count, double lat, double lon, WaypointList wptList)
        {
            var x = WaypointAirwayConnector.FindAirwayConnection(lat, lon, wptList);
            var result = new List<int>(x.Count);

            foreach (var i in x)
            {
                result.Add(i.Index);
            }
            return result;
        }

        /// <summary>
        /// Remove any array which is null, or has less elements than minLength.
        /// </summary>
        public static void RemoveTinyArray<T>(this List<T[]> item, int minLength)
        {
            int lastIndex = item.Count - 1;

            if (lastIndex < 0)
            {
                return;
            }

            for (int i = lastIndex; i >= 0; i--)
            {
                if (item[i] == null || item[i].Length < minLength)
                {
                    item.RemoveAt(i);
                }
            }
        }

        public static List<string[]> SelectDistinct(List<string[]> item)
        {
            return item.Distinct(new ArrayComparer<string>()).ToList();
        }
    }
}
