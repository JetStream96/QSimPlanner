using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    public class StarInfo
    {
        public double TotalDistance { get; private set; }
        public Waypoint FirstWaypoint { get; private set; }

        public StarInfo(double TotalDistance, Waypoint FirstWaypoint)
        {
            this.TotalDistance = TotalDistance;
            this.FirstWaypoint = FirstWaypoint;
        }
    }

}
