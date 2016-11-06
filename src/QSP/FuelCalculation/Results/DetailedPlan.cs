using QSP.FuelCalculation.Calculations;
using System.Collections.Generic;
using System.Linq;
using QSP.FuelCalculation.Results.Nodes;

namespace QSP.FuelCalculation.Results
{
    public class DetailedPlan
    {
        public IReadOnlyList<PlanNode> AllNodes { get; }

        public IEnumerable<PlanNode> PrintedNodes =>
            AllNodes.Where(n => !(n.NodeValue is IntermediateNode));

        public DetailedPlan(IReadOnlyList<PlanNode> AllNodes)
        {
            this.AllNodes = AllNodes;
        }
    }
}
