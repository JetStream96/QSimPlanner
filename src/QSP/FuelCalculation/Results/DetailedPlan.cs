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

    // The units of variables used in this class is specified in 
    // FuelCalculation/Calculations/VariableUnitStandard.txt.

    public static class DetailedPlanExtension
    {
        public static double AirDistance(this IReadOnlyList<PlanNode> n)
        {
            var dis = 0.0;

            for (int i = 0; i < n.Count - 1; i++)
            {
                var tas = (n[i].Ktas + n[i + 1].Ktas) / 2.0;
                var time = n[i].TimeRemaining - n[i + 1].TimeRemaining;
                dis += tas * time / 60.0;
            }

            return dis;
        }
    }
}
