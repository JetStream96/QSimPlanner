using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System.Windows.Forms;
using static QSP.MathTools.Numbers;

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
            double totalDis = route.TotalDistance();
            int disInt = RoundToInt(totalDis);
            double directDis = route.FirstWaypoint.Distance(
                route.LastWaypoint);
            double percentDiff = (totalDis - directDis) / directDis * 100.0;
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
