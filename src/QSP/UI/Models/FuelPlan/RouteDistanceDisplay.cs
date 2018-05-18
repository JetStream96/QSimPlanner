using QSP.MathTools;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;

namespace QSP.UI.Models.FuelPlan
{
    public enum Style
    {
        Short,
        Long
    }

    public static class RouteDistanceDisplay
    {
        public static string GetDisplay(Route route, Style displayStyle)
        {
            double totalDis = route.TotalDistance();
            int disInt = Numbers.RoundToInt(totalDis);
            double directDis = route.FirstWaypoint.Distance(route.LastWaypoint);
            double percentDiff = (totalDis - directDis) / directDis * 100.0;
            string diffStr = percentDiff.ToString("0.0");

            var text = $"{disInt} NM (+{diffStr}%)";
            return displayStyle == Style.Long ? ("Distance: " + text) : text;
        }
    }
}
