using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.FuelCalculation.Results.Nodes;

namespace QSP.FuelCalculation.Results
{
    public class PlanNode
    {
        // Remember to update Coordinate property getter if this is changed.
        // All allowed types of NodeValue must return the correct ICoordinate.
        public static readonly IReadOnlyList<Type> AllowedNodeTypes =
            new Type[] 
            {
                typeof(RouteNode) ,
                typeof(IntermediateNode),
                typeof(TocNode),
                typeof(TodNode)
            };

        public object NodeValue { get; private set; }
        public double TimeRemainingMin { get; private set; }
        public double AltitudeFt { get; private set; }
        public double TasKnots { get; private set; }
        public double GSKnots { get; private set; }
        public double FuelOnBoardKg { get; private set; }

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

                throw new InvalidOperationException(
                    "Something is wrong in NodeValue validation.");
            }
        }

        public PlanNode(
            object NodeValue,
            double TimeRemainingMin,
            double AltitudeFt,
            double TasKnots,
            double GSKnots,
            double FuelOnBoardKg)
        {
            if (!IsValidType(NodeValue))
            {
                throw new ArgumentException("Type not allowed.");
            }

            this.NodeValue = NodeValue;
            this.TimeRemainingMin = TimeRemainingMin;
            this.AltitudeFt = AltitudeFt;
            this.TasKnots = TasKnots;
            this.GSKnots = GSKnots;
            this.FuelOnBoardKg = FuelOnBoardKg;
        }

        private static bool IsValidType(object NodeValue)
        {
            var type = NodeValue.GetType();
            return AllowedNodeTypes.Any(t => t.IsAssignableFrom(type));
        }
    }
}
