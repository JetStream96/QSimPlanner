using QSP.LibraryExtension;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Data.Interfaces;
using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Common
{
    public static class Utilities
    {
        public static int GetClosest(
            double prevLat,
            double prevLon,
            List<int> candidates,
            WaypointList wptList)
        {
            return candidates
                .MinBy(i => wptList[i].Distance(prevLat, prevLon));
        }
    }
}
