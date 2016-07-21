using QSP.RouteFinding.Containers;
using System.Collections.Generic;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public class SidWaypoints
    {
        private List<Waypoint> _wpts;

        public IReadOnlyList<Waypoint> Waypoints
        {
            get
            {
                return _wpts;
            }
        }

        public bool EndsWithVector { get; private set; }

        public SidWaypoints(List<Waypoint> Waypoints, bool EndsWithVector)
        {
            this._wpts = Waypoints;
            this.EndsWithVector = EndsWithVector;
        }
    }
}
