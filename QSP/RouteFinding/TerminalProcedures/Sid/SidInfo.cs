namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public class SidInfo
    {
        public double TotalDistance { get; private set; }
        public Waypoint LastWaypoint { get; private set; }
        public bool EndsWithVector { get; private set; }

        public SidInfo(double TotalDistance, Waypoint LastWaypoint, bool EndsWithVector)
        {
            this.TotalDistance = TotalDistance;
            this.LastWaypoint = LastWaypoint;
            this.EndsWithVector = EndsWithVector;
        }
    }
}
