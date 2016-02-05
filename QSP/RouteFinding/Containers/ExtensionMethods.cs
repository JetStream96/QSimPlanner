using static QSP.MathTools.Utilities;

namespace QSP.RouteFinding.Containers
{
    public static class ExtensionMethods
    {
        public static double DistanceFrom(this Waypoint wpt, Waypoint item)
        {
            return GreatCircleDistance(wpt.Lat, wpt.Lon, item.Lat, item.Lon);
        }
    }
}
