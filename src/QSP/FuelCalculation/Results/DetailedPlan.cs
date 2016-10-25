using System.Collections.Generic;
using QSP.RouteFinding.Routes;

namespace QSP.FuelCalculation.Results
{
    public class DetailedPlan
    {
        public IReadOnlyList<PlanNode> Nodes { get; private set; }

        public DetailedPlan(IReadOnlyList<PlanNode> Nodes)
        {
            this.Nodes = Nodes;
        }
    }
}
