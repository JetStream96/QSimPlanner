using System.Collections.Generic;
using QSP.FuelCalculation.Results;
using QSP.MathTools.Vectors;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using static QSP.AviationTools.SpeedConversion;
using static QSP.WindAloft.GroundSpeedCalculation;

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

        // These are passed in via ctor.
        public IWindTableCollection WindTable { get; }
        public Waypoint PrevWaypoint { get; }
        public LinkedListNode<RouteNode> NextRouteNode { get; }
        public ICoordinate NextPlanNodeCoordinate { get; }
        public double Alt { get; }
        public double GrossWt { get; }
        public double FuelOnBoard { get; }
        public double TimeRemaining { get; }
        public double Kias { get; }

        // These are computed in the class.
        public double Ktas { get; private set; } 
        public double Gs { get; private set; }

        public PlanNodeState(
            IWindTableCollection WindTable,
            Waypoint PrevWaypoint,
            LinkedListNode<RouteNode> NextRouteNode,
            ICoordinate NextPlanNodeCoordinate,
            double Alt,
            double GrossWt,
            double FuelOnBoard,
            double TimeRemaining,
            double Kias)
        {
            this.WindTable = WindTable;
            this.PrevWaypoint = PrevWaypoint;
            this.NextRouteNode = NextRouteNode;
            this.NextPlanNodeCoordinate = NextPlanNodeCoordinate;
            this.Alt = Alt;
            this.GrossWt = GrossWt;
            this.FuelOnBoard = FuelOnBoard;
            this.TimeRemaining = TimeRemaining;
            this.Kias = Kias;

            ComputeParameters();
        }

        private void ComputeParameters()
        {
            Ktas = Ktas(Kias, Alt);
            Gs = GetGS(
                WindTable, 
                Alt,
                Ktas,
                PrevWaypoint.ToVector3D(), 
                NextRouteNode.Value.Waypoint.ToVector3D(),
                NextPlanNodeCoordinate.ToVector3D());
        }
    }
}