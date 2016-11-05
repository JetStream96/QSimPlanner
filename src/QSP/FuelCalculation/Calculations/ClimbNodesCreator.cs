using System;
using System.Collections.Generic;
using System.Linq;
using QSP.FuelCalculation.FuelDataNew;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using static QSP.AviationTools.Constants;
using static QSP.AviationTools.SpeedConversion;

namespace QSP.FuelCalculation.Calculations
{
    public class ClimbNodesCreator
    {
        private readonly AirportManager airportList;
        private readonly Route route;
        private readonly FuelDataNew.FuelDataItem fuelData;
        private readonly IReadOnlyList<PlanNode> initPlan;

        public ClimbNodesCreator(
            AirportManager airportList,
            Route route,
            FuelDataNew.FuelDataItem fuelData,
            IReadOnlyList<PlanNode> initPlan)
        {
            this.airportList = airportList;
            this.route = route;
            this.fuelData = fuelData;
            this.initPlan = initPlan;
        }

        public List<PlanNode> Create()
        {
            var estimation = NodeEstimation();

            // Fix GrossWt, FuelOnBoard, TimeRemaining.
            var last = estimation.Last();
            var old = initPlan[estimation.Count - 1];
            var grossWtShift = old.GrossWt - last.GrossWt;
            var fuelShift = old.FuelOnBoard - last.FuelOnBoard;
            var timeShift = old.TimeRemaining - last.TimeRemaining;

            return estimation
                .Select(n => GetNode(
                    n,
                    n.Alt,
                    n.GrossWt + grossWtShift,
                    n.FuelOnBoard + fuelShift,
                    n.TimeRemaining + timeShift))
                .ToList();
        }

        private List<PlanNode> NodeEstimation()
        {
            // We uses the node provided by initPlan.
            // In initPlan, the climb segment is not calculated.
            // We estimate the gross weight at origin airport and compute
            // the climb segment.
            // 
            // In climbNodes, the absolute values of the following parameters 
            // are probably not correct but the relative values between nodes
            // are accurate: GrossWt, FuelOnBoard, TimeRemaining.
            //

            var climbNodes = new List<PlanNode>();
            var oldNode = initPlan[0];

            var prevPlanNode = GetNode(
                oldNode,
                OrigElevationFt(),
                oldNode.GrossWt,
                oldNode.FuelOnBoard,
                0.0);

            climbNodes.Add(prevPlanNode);

            while (prevPlanNode.Alt < oldNode.Alt)
            {
                if (climbNodes.Count == initPlan.Count)
                {
                    throw new InvalidOperationException(
                        "The altitude difference between origin and " +
                        "destination airports is too large for this route.");
                }

                oldNode = initPlan[climbNodes.Count];
                prevPlanNode = NextPlanNode(prevPlanNode, oldNode);
                climbNodes.Add(prevPlanNode);
            }

            return climbNodes;
        }

        private PlanNode NextPlanNode(PlanNode prev, PlanNode old)
        {
            var ff = fuelData.ClimbFuelFlow(prev.GrossWt);
            var stepDis = prev.Distance(prev.NextPlanNodeCoordinate);
            var stepTime = stepDis / prev.Gs * 60.0;
            var stepFuel = stepTime * ff;
            var climbGrad = fuelData.ClimbGradient(prev.GrossWt);
            var climbRate = climbGrad * prev.Ktas / 60.0 * NmFtRatio;
            var alt = Math.Min(prev.Alt + stepTime * climbRate, old.Alt);
            var grossWt = prev.GrossWt - stepFuel;
            var fuelOnBoard = prev.FuelOnBoard - stepFuel;
            var timeRemaining = prev.TimeRemaining - stepTime;

            return GetNode(old, alt, grossWt, fuelOnBoard, timeRemaining);
        }

        private PlanNode GetNode(PlanNode old,
            double alt, double grossWt, double fuelOnBoard, double timeRemain)
        {
            return new PlanNode(
                old.NodeValue,
                old.WindTable,
                old.NextRouteNode,
                old.NextPlanNodeCoordinate,
                alt,
                grossWt,
                fuelOnBoard,
                timeRemain,
                Kias(grossWt, alt));
        }

        private double Kias(double grossWt, double alt)
        {
            var cruiseKias = fuelData.CruiseKias(grossWt);
            var optAlt = fuelData.OptCruiseAlt(grossWt);
            var optCruiseKtas = Ktas(cruiseKias, optAlt);
            var kias = fuelData.ClimbKias;
            var ktas = Ktas(kias, alt);
            return ktas > optCruiseKtas ? KtasToKcas(optCruiseKtas, alt) : kias;
        }

        private double OrigElevationFt()
        {
            var icao = route.First.Value.Waypoint.ID.Substring(0, 4).ToUpper();
            return airportList[icao].Elevation;
        }
    }
}