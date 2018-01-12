using System.Linq;
using QSP.FuelCalculation.FuelData;
using QSP.FuelCalculation.Results;

namespace QSP.FuelCalculation.Calculations.Corrections
{
    public static class FuelCorrection
    {
        /// <summary>
        /// The argument 'plan' should be the return value of FuelCalculator.Create() 
        /// method. Note that the fuel consumption depends on the weight of the aircraft,
        /// which depends on the vertical profile of the flight plan. Because of this,
        /// when FuelCalculator creates the DetailedPlan, the fuel values for 
        /// nodes in climb segment are estimations.
        /// 
        /// Use this method to compute the exact fuel amounts for each node.
        /// </summary>
        public static DetailedPlan ApplyCorrection(this DetailedPlan plan, FuelDataItem item)
        {
            if (item.FuelTable == null) return plan;
            var nodes = plan.AllNodes;
            var airDis = nodes.AirDistance();
            var exactFuel = item.FuelTable.FuelRequired(airDis, nodes.Last().GrossWt);
            var landingFuel = nodes.Last().FuelOnBoard;
            var factor = exactFuel / (nodes[0].FuelOnBoard - landingFuel);
            var zfw = nodes[0].GrossWt - nodes[0].FuelOnBoard;
            return new DetailedPlan(nodes.Select(n =>
            {
                var newFuel = (n.FuelOnBoard - landingFuel) * factor + landingFuel;
                return GetNode(n, newFuel, newFuel + zfw);
            }).ToList());
        }

        private static IPlanNode GetNode(IPlanNode old, double fuelOnBoard, double grossWt)
        {
            var node = PlanNode.Copy(old);
            node.FuelOnBoard = fuelOnBoard;
            node.GrossWt = grossWt;
            return node;
        }
    }
}