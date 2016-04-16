using System.Collections.Generic;
using System.Collections.ObjectModel;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public class SidWaypoints
    {
        private List<Waypoint> _wpts;

        public ReadOnlyCollection<Waypoint> Waypoints
        {
            get
            {
                return _wpts.AsReadOnly();
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
