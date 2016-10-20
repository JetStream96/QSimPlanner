using QSP.RouteFinding.Routes;

namespace UnitTest.RouteFinding.RouteAnalyzers
{
    public static class Util
    {
        public static RouteString GetRouteString(params string[] x)
        {
            return x.ToRouteString();
        }
    }
}
