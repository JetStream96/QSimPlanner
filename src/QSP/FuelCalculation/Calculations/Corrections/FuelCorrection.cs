using System.Linq;
using QSP.FuelCalculation.FuelData;
using QSP.FuelCalculation.Results;

namespace QSP.FuelCalculation.Calculations.Corrections
{
    public static class FuelCorrection
    {
        public static DetailedPlan ApplyCorrection(this DetailedPlan plan, FuelDataItem item)
        {
            if (item.FuelTable == null) return plan;
            var nodes = plan.AllNodes;
            var airDis = nodes.AirDistance();
            var exactFuel = item.FuelTable.FuelRequired(airDis, nodes.Last().GrossWt);
            var landingFuel = nodes.Last().FuelOnBoard;
            var factor = (nodes[0].FuelOnBoard - landingFuel) / exactFuel;
            var zfw = nodes[0].GrossWt - nodes[0].FuelOnBoard;
            return new DetailedPlan(nodes.Select(n =>
            {
                var newFuel = (n.FuelOnBoard - landingFuel) * factor + landingFuel;
                return GetNode(n, newFuel, newFuel + zfw);
            }).ToList());
        }

        private static PlanNode GetNode(PlanNode old, double fuelOnBoard, double grossWt)
        {
            return new PlanNode(
                old.NodeValue,
                old.WindTable,
                old.NextRouteNode,
                old.NextPlanNodeCoordinate,
                old.Alt,
                grossWt,
                fuelOnBoard,
                old.TimeRemaining,
                old.Kias,
                old.Ktas,
                old.Gs);
        }
    }
}