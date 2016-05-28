using static QSP.MathTools.GCDis;

namespace QSP.RouteFinding.Containers
{
    public static class WaypointExtensions
    {
        public static double DistanceFrom(this Waypoint wpt, Waypoint item)
        {
            return Distance(wpt.Lat, wpt.Lon, item.Lat, item.Lon);
        }
    }
}
