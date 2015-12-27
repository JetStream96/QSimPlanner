using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    public class SidInfo
    {
        private double _totalDis;
        private Waypoint _lastWpt;

        public double TotalDistance
        {
            get
            {
                return _totalDis;
            }
        }

        public Waypoint LastWaypoint
        {
            get
            {
                return _lastWpt;
            }
        }

        public SidInfo(double totalDistance, Waypoint lastWaypoint)
        {
            _totalDis = totalDistance;
            _lastWpt = lastWaypoint;
        }
    }
}
