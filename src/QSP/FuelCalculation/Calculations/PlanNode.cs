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

    public class PlanNode : IPlanNode
    {
        // Remember to update GetCoordinate() if more types are added to this 
        // field.
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

        // If the current node is at a RouteNode, this property
        // is identical to the current node.
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

        // If the current node is at a waypoint, this property is the one
        // before current node. This is null if current node is the first
        // node of the route.
        public Waypoint PrevWaypoint => PrevRouteNode?.Value.Waypoint;

        // If the current node is at a RouteNode, this property
        // is the one before the current node. Therefore, this will be null
        // if current node is the first node of the route.
        public LinkedListNode<RouteNode> PrevRouteNode =>
            NextRouteNode.Previous;

        public double Lat { get; private set; }
        public double Lon { get; private set; }

        public PlanNode(
            object NodeValue,
            IWindTableCollection WindTable,
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
            var c = GetCoordinate();
            Lat = c.Lat;
            Lon = c.Lon;

            Ktas = Ktas(Kias, Alt);

            Gs = GetGS(
                WindTable,
                Alt,
                Ktas,
                (PrevWaypoint ?? (ICoordinate)this).ToVector3D(),     // *
                NextPlanNodeCoordinate.ToVector3D(),
                this.ToVector3D());

            // * NextPlanNodeCoordinate is different from Coordinate if the
            //   current node is not the first node.
        }

        private ICoordinate GetCoordinate()
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
}