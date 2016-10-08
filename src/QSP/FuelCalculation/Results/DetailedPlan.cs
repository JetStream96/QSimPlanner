using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Containers;

namespace QSP.FuelCalculation.Results
{
    public class DetailedPlan
    {
        public IReadOnlyList<Node> Nodes { get; private set; }

        public DetailedPlan(IReadOnlyList<Node> Nodes)
        {
            this.Nodes = Nodes;
        }

        public class Node
        {
            public RouteNode RouteNode { get; private set; }
            public double TimeRemainingMin { get; private set; }
            public double AltitudeFt { get; private set; }
            public double TasKnots { get; private set; }
            public double FuelOnBoardTon { get; private set; }
            
            public Node(
                RouteNode RouteNode,
                double TimeRemainingMin,
                double AltitudeFt,
                double TasKnots,
                double FuelOnBoardTon)
            {
                this.RouteNode = RouteNode;
                this.TimeRemainingMin = TimeRemainingMin;
                this.AltitudeFt = AltitudeFt;
                this.TasKnots = TasKnots;
                this.FuelOnBoardTon = FuelOnBoardTon;
            }
        }
    }
}
