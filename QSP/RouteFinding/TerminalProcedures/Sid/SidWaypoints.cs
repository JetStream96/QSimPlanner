using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

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
