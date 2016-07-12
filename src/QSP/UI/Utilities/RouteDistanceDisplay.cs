using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;

namespace QSP.UI.Utilities
{
    public static class RouteDistanceDisplay
    {
        public enum DistanceDisplayStyle
        {
            Short,
            Long
        }

        public static void UpdateRouteDistanceLbl(
            Label lbl, Route route, DistanceDisplayStyle displayStyle)
        {
            double totalDis = route.GetTotalDistance();
            int disInt = RoundToInt(totalDis);
            double directDis =
                route.FirstWaypoint.DistanceFrom(route.LastWaypoint);
            double percentDiff = (totalDis - directDis) / directDis * 100;
            string diffStr = percentDiff.ToString("0.0");
            
            var text = $"{disInt} NM (+{diffStr}%)";

            if (displayStyle == DistanceDisplayStyle.Long)
            {
                text = "Distance: " + text;
            }

            lbl.Text = text;
        }
    }
}
