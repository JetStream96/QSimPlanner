using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using System.Collections.Generic;
using static QSP.MathTools.GCDis;
using static QSP.RouteFinding.Constants;

namespace QSP.RouteFinding
{
    // Connect isolated waypoints to an airway, by finding 
    // nearby waypoints which are connected to airway(s).
    //
    public static class WaypointAirwayConnector
    {
        public const double SearchRangeIncr = 20.0;
        public const double MAX_SEARCH_RANGE = MAX_LEG_DIS;
        public const int TARGET_NUM = 30;

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
            return FindAirwayConnection(Lat, Lon, wptList, MAX_SEARCH_RANGE, TARGET_NUM, SearchRangeIncr);
        }

        /// <summary>
        /// Finds a list of waypoints which is near the given Lat/Lon, and are connected to at least one other waypoint.
        /// </summary>
        public static List<IndexDistancePair> FindAirwayConnection(double Lat, double Lon, WaypointList wptList,
                                                                   double maxSearchRange, double targetNumber)
        {
            return FindAirwayConnection(Lat, Lon, wptList, maxSearchRange, targetNumber, SearchRangeIncr);
        }

        /// <summary>
        /// Finds a list of waypoints which is near the given Lat/Lon, and are connected to at least one other waypoint.
        /// </summary>
        public static List<IndexDistancePair> FindAirwayConnection(double Lat, double Lon, WaypointList wptList, double maxSearchRange,
                                                                   double targetNumber, double searchRangeIncrement)
        {   
            double searchRange = 0.0;
            var result = new List<IndexDistancePair>();

            while (searchRange <= maxSearchRange && result.Count < targetNumber)
            {
                result.Clear();
                searchRange += searchRangeIncrement;

                var searchResult = wptList.Find(Lat, Lon, searchRange);
                double dctDis;

                foreach (var item in searchResult)
                {
                    int i = item.Index;

                    if (wptList.EdgesFromCount(i) > 0)  // TODO: SID, STAR should be treated seperately
                    {
                        dctDis = Distance(wptList[i].Lat, wptList[i].Lon, Lat, Lon);
                        result.Add(new IndexDistancePair(i, dctDis));
                    }
                }
            }
            return result;
        }
    }
}
