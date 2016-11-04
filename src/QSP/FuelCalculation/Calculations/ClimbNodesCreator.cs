using System;
using System.Collections.Generic;
using QSP.FuelCalculation.FuelDataNew;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using static QSP.AviationTools.Constants;

namespace QSP.FuelCalculation.Calculations
{
    public class ClimbNodesCreator
    {
        private static readonly double deltaT = 0.5;
        private static readonly double altDiffCriteria = 0.1;

        private readonly AirportManager airportList;
        private readonly ICrzAltProvider altProvider;
        private readonly IWindTableCollection windTable;
        private readonly Route route;
        private readonly FuelDataNew.FuelDataItem fuelData;
        private readonly double zfw;
        private readonly double landingFuel;
        private readonly double maxAlt;

        public ClimbNodesCreator(
            AirportManager airportList,
            ICrzAltProvider altProvider,
            IWindTableCollection windTable,
            Route route,
            FuelDataNew.FuelDataItem fuelData,
            double zfw,
            double landingFuel,
            double maxAlt)
        {
            if (route.Count < 2) throw new ArgumentException();

            this.airportList = airportList;
            this.altProvider = altProvider;
            this.windTable = windTable;
            this.route = route;
            this.fuelData = fuelData;
            this.zfw = zfw;
            this.landingFuel = landingFuel;
            this.maxAlt = maxAlt;
        }

        public List<PlanNode> Create(IReadOnlyList<PlanNode> initPlan)
        {
            // We uses the node provided by initPlan.
            // In initPlan, the climb segment is not calculated.
            // We estimate the gross weight at origin airport and compute
            // the climb segment.
            // 
            // In climbNodes, the absolute values of the following parameters 
            // are probably not correct but the relative values between nodes
            // are accurate: GrossWt, FuelOnBoard, TimeRemaning.
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

                var old = initPlan[climbNodes.Count];
                prevPlanNode = NextPlanNode(prevPlanNode, old);
                climbNodes.Add(prevPlanNode);
            }

            return climbNodes;
        }

        private PlanNode NextPlanNode(PlanNode prev, PlanNode old)
        {
            var ff = fuelData.CruiseFuelFlow(prev.GrossWt);
            var stepDis = prev.Coordinate.Distance(prev.NextPlanNodeCoordinate);
            var stepTime = stepDis / prev.Gs;
            var stepFuel = stepTime * ff;
            var climbGrad = fuelData.ClimbGradient(prev.GrossWt);
            var climbRate = climbGrad * prev.Ktas / 60.0 * NmFtRatio;
            double alt = prev.Alt + stepTime * climbRate;
            double grossWt = prev.GrossWt - stepFuel;
            double fuelOnBoard = prev.FuelOnBoard - stepFuel;
            double timeRemaining = prev.TimeRemaining - stepTime;

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
                fuelData.ClimbKias);
        }

        private double OrigElevationFt()
        {
            var icao = route.First.Value.Waypoint.ID.Substring(0, 4).ToUpper();
            return airportList[icao].Elevation;
        }
    }
}