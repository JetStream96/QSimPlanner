using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public class SidWaypoints
    {
        public List<Waypoint> Waypoints { get; private set; }
        public bool EndsWithVector { get; private set; }

        public SidWaypoints(List<Waypoint > Waypoints,bool EndsWithVector)
        {
            this.Waypoints = Waypoints;
            this.EndsWithVector = EndsWithVector;
        }
    }
}
