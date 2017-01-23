using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // This class is immutable.
    public class RouteSegment
    {
        public bool IsAuto { get; }
        public bool IsRand { get; }
        public RouteString RouteString { get; }

        public static RouteSegment Auto()
        {
            return new RouteSegment(true, false, null);
        }

        public static RouteSegment Rand()
        {
            return new RouteSegment(false, true, null);
        }

        public RouteSegment(RouteString RouteString) : this(false, false, RouteString) { }

        private RouteSegment(bool IsAuto, bool IsRand, RouteString RouteString)
        {
            this.IsAuto = IsAuto;
            this.IsRand = IsRand;
            this.RouteString = RouteString;
        }
    }
}
