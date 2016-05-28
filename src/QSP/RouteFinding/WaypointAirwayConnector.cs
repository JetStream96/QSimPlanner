using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Data;
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
        public const double MaxSearchRange = MaxLegDis;
        public const int TargetCount = 30;

        public static List<IndexDistancePair> FindAirwayConnection(
            double Lat,
            double Lon,
            WaypointList wptList)
        {
            return FindAirwayConnection(
                Lat, Lon, wptList, new WptSearchOption());
        }

        /// <summary>
        /// Finds a list of waypoints which is near the given 
        /// Lat/Lon, and are connected to at least one other waypoint.
        /// </summary>
        public static List<IndexDistancePair> FindAirwayConnection(
            double Lat,
            double Lon,
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
