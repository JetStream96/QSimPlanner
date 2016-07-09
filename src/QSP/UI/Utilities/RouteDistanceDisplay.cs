using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;

namespace QSP.UI.Utilities
{
    public static class RouteDistanceDisplay
    {
        public static void UpdateRouteDistanceLbl(Label lbl, Route route)
        {
            double totalDis = route.GetTotalDistance();
            int disInt = RoundToInt(totalDis);
            double directDis =
                route.FirstWaypoint.DistanceFrom(route.LastWaypoint);
            double percentDiff = (totalDis - directDis) / directDis * 100;
            string diffStr = percentDiff.ToString("0.0");

            lbl.Text = $"Total Distance: {disInt} NM (+{diffStr}%)";
        }
    }
}
