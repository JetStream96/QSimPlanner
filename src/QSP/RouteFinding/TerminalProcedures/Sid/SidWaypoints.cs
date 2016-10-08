using QSP.RouteFinding.Containers;
using System.Collections.Generic;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public class SidWaypoints
    {
        public IReadOnlyList<Waypoint> Waypoints { get; private set; }
        public bool EndsWithVector { get; private set; }

        public SidWaypoints(IReadOnlyList<Waypoint> Waypoints, 
            bool EndsWithVector)
        {
            this.Waypoints = Waypoints;
            this.EndsWithVector = EndsWithVector;
        }
    }
}
