using System;
using System.Collections.Generic;
using System.Linq;
using QSP.Common;
using QSP.FuelCalculation.Results.Nodes;
using QSP.MathTools.Vectors;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using static QSP.AviationTools.SpeedConversion;
using static QSP.WindAloft.GroundSpeedCalculation;

namespace QSP.FuelCalculation.Calculations
{
    // The units of variables used in this class is specified in 
    // VariableUnitStandard.txt.

    public class PlanNode
    {
        // TODO: Are these types all used?
        // Remember to update Coordinate property getter if this is changed.
        // All allowed types of NodeValue must return the correct ICoordinate.
        public static readonly IEnumerable<Type> AllowedNodeTypes = new[]
        {
            typeof(RouteNode),
            typeof(IntermediateNode),
            typeof(TocNode),
            typeof(TodNode),
            typeof(ScNode)
        };

        public object NodeValue { get; }

        // Here 'previous' and 'next' refers to the order of nodes/waypoints
        // in route. Do not confuse with the order of calculation although some
        // classes like InitialPlanCreator computes the flight plan backwards.

        // These are passed in via ctor.
        public IWindTableCollection WindTable { get; }

        // If the current node is at a waypoint, this property is the one
        // before current node.
        public Waypoint PrevWaypoint { get; }

        // If the current node is at a RouteNode, the next two properties
        // are identical to the current node.
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

        public ICoordinate Coordinate
        {
            get
            {
                var intermediateNode = NodeValue as IntermediateNode;
                if (intermediateNode != null) return intermediateNode.Coordinate;

                var routeNode = NodeValue as RouteNode;
                if (routeNode != null) return routeNode.Waypoint;

                var tocNode = NodeValue as TocNode;
                if (tocNode != null) return tocNode.Coordinate;

                var todNode = NodeValue as TodNode;
                if (todNode != null) return todNode.Coordinate;

                var scNode = NodeValue as ScNode;
                if (scNode != null) return scNode.Coordinate;

                throw new UnexpectedExecutionStateException(
                    "Something is wrong in NodeValue validation.");
            }
        }

        public PlanNode(
            object NodeValue,
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
            if (!IsValidType(NodeValue))
            {
                throw new ArgumentException("Type not allowed.");
            }

            this.NodeValue = NodeValue;
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

        private static bool IsValidType(object NodeValue)
        {
            var type = NodeValue.GetType();
            return AllowedNodeTypes.Any(t => t.IsAssignableFrom(type));
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