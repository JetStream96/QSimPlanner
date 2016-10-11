using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Data;
using QSP.RouteFinding.Data.Interfaces;
using System.Collections.Generic;

namespace QSP.RouteFinding
{
    // Connect isolated waypoints to an airway, by finding 
    // nearby waypoints which are connected to airway(s).
    //
    public static class WaypointAirwayConnector
    {
        public static List<IndexDistancePair> ToAirway(
            double Lat,
            double Lon,
            WaypointList wptList)
        {
            return ToAirway(Lat, Lon, wptList, new WptSearchOption());
        }

        public static List<IndexDistancePair> FromAirway(
            double Lat,
            double Lon,
            WaypointList wptList)
        {
            return FromAirway(Lat, Lon, wptList, new WptSearchOption());
        }

        public static List<IndexDistancePair> ToAirway(
            double Lat,
            double Lon,
            WaypointList wptList,
            WptSearchOption option)
        {
            return Find(Lat, Lon, true, wptList, option);
        }

        public static List<IndexDistancePair> FromAirway(
            double Lat,
            double Lon,
            WaypointList wptList,
            WptSearchOption option)
        {
            return Find(Lat, Lon, false, wptList, option);
        }

        // Finds a list of waypoints which is near the given 
        // Lat/Lon, and are connected to at least one other waypoint.
        private static List<IndexDistancePair> Find(
            double Lat,
            double Lon,
            bool IsSid,
            WaypointList wptList,
            WptSearchOption option)
        {
            double searchRange = 0.0;
            var result = new List<IndexDistancePair>();

            while (searchRange <= option.MaxSearchRange &&
                result.Count < option.TargetCount)
            {
                result.Clear();
                searchRange += option.SearchRangeIncr;
                var searchResult = wptList.Find(Lat, Lon, searchRange);

                foreach (var item in searchResult)
                {
                    int i = item.Index;

                    if ((IsSid && wptList.EdgesFromCount(i) > 0) ||
                        (IsSid == false && wptList.EdgesToCount(i) > 0))
                    {
                        double dctDis = wptList[i].Distance(Lat, Lon);
                        result.Add(new IndexDistancePair(i, dctDis));
                    }
                }
            }

            return result;
        }
    }
}
