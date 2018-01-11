using QSP.FuelCalculation.FuelData;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Constants;
using static QSP.AviationTools.SpeedConversion;

namespace QSP.FuelCalculation.Calculations
{
    // The units of variables used in this class is specified in 
    // VariableUnitStandard.txt.

    public class ClimbNodesCreator
    {
        private readonly AirportManager airportList;
        private readonly Route route;
        private readonly FuelDataItem fuelData;
        private readonly IReadOnlyList<IPlanNode> initPlan;

        public ClimbNodesCreator(
            AirportManager airportList,
            Route route,
            FuelDataItem fuelData,
            IReadOnlyList<IPlanNode> initPlan)
        {
            this.airportList = airportList;
            this.route = route;
            this.fuelData = fuelData;
            this.initPlan = initPlan;
        }

        /// <exception cref="ElevationDifferenceTooLargeException">
        /// Elevation difference between origin and destination is too large for
        /// aircraft's climb or descent performance.
        /// </exception>
        public List<IPlanNode> Create()
        {
            const int iterationCount = 2;
            var initGrossWt = initPlan[0].GrossWt;
            var result = new List<IPlanNode>();

            for (int i = 0; i < iterationCount; i++)
            {
                var estimation = NodeEstimation(initGrossWt);

                // Fix GrossWt, FuelOnBoard, TimeRemaining.
                var last = estimation.Last();
                var old = initPlan[estimation.Count - 1];
                var grossWtShift = old.GrossWt - last.GrossWt;
                var fuelShift = old.FuelOnBoard - last.FuelOnBoard;
                var timeShift = old.TimeRemaining - last.TimeRemaining;

                result = estimation
                    .Select(n => GetNode(
                        n,
                        n.Alt,
                        n.GrossWt + grossWtShift,
                        n.FuelOnBoard + fuelShift,
                        n.TimeRemaining + timeShift))
                    .ToList();

                initGrossWt = result[0].GrossWt;
            }

            return result;
        }
        
        /// <exception cref="ElevationDifferenceTooLargeException">
        /// Elevation difference between origin and destination is too large for
        /// aircraft's climb or descent performance.
        /// </exception>
        private List<IPlanNode> NodeEstimation(double initGrossWt)
        {
            // We uses the node provided by initPlan.
            // In initPlan, the climb segment is not calculated.
            // We estimate the gross weight at origin airport and compute
            // the climb segment.
            // 
            // In climbNodes, the values of the following parameters are 
            // probably not exactly correct, but the relative values between nodes
            // are accurate: GrossWt, FuelOnBoard, TimeRemaining.
            //

            var climbNodes = new List<IPlanNode>();
            var oldNode = initPlan[0];
            var zfw = oldNode.GrossWt - oldNode.FuelOnBoard;

            var prevPlanNode = GetNode(
                oldNode,
                OrigElevationFt(),
                initGrossWt,
                initGrossWt - zfw,
                0.0);

            climbNodes.Add(prevPlanNode);

            while (prevPlanNode.Alt < oldNode.Alt)
            {
                if (climbNodes.Count == initPlan.Count)
                {
                    throw new ElevationDifferenceTooLargeException(
                        "The altitude difference between origin and " +
                        "destination airports is too large for this route.");
                }

                oldNode = initPlan[climbNodes.Count];
                prevPlanNode = NextPlanNode(prevPlanNode, oldNode);
                climbNodes.Add(prevPlanNode);
            }

            return climbNodes;
        }

        private IPlanNode NextPlanNode(IPlanNode prev, IPlanNode old)
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

        private IPlanNode GetNode(IPlanNode old,
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
            if (alt <= 10000.0) return 250.0;
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