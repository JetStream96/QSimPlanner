using QSP.FuelCalculation.Calculations;
using System.Collections.Generic;
using System.Linq;
using QSP.FuelCalculation.Results.Nodes;

namespace QSP.FuelCalculation.Results
{
    public class DetailedPlan
    {
        public IReadOnlyList<IPlanNode> AllNodes { get; }

        public IEnumerable<IPlanNode> PrintedNodes =>
            AllNodes.Where(n => !(n.NodeValue is IntermediateNode));

        public DetailedPlan(IReadOnlyList<IPlanNode> AllNodes)
        {
            this.AllNodes = AllNodes;
        }
    }
}
