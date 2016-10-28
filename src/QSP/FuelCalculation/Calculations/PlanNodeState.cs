using System.Collections.Generic;
using QSP.MathTools.Vectors;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;

namespace QSP.FuelCalculation.Calculations
{
    public class PlanNodeState
    {
        // Variable units:
        // Altitude: ft
        // Time: min
        // Distance: nm
        // Speed: knots
        // Climb/Descent rate: ft/min
        // Weight: kg
        // Fuel amount: kg
        // Fuel flow: kg/min

        // Here 'previous' and 'next' refers to the order of nodes/waypoints
        // in route. Do not confuse with the order of calculation although some
        // classes like InitialPlanCreator computes the flight plan backwards.

        public LinkedListNode<RouteNode> PrevNode { get; }
        public Waypoint NextWaypoint { get; }
        public Vector3D V { get; }
        public double Alt { get; }
        public double GrossWt { get; }
        public double FuelOnBoard { get; }
        public double TimeRemaining { get; }
        public double Kias { get; }
        public double Ktas { get; }
        public double Gs { get; }


    }
}