using QSP.RouteFinding.Routes;

namespace QSP.WindAloft
{
    public static class AvgWindCalculatorExtension
    {
        public static double GetAirDistance(
            this AvgWindCalculator calc,
            Route route)
        {
            double AirDis = 0.0;
            var node = route.First;

            while (node != route.Last)
            {
                var result = calc.GetAvgWind(
                    node.Value.Waypoint,
                    node.Next.Value);

                AirDis += result.AirDis;
                node = node.Next;
            }

            return AirDis;
        }
    }
}
