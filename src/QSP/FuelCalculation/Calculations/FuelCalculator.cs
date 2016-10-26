using System;
using System.Collections.Generic;
using QSP.WindAloft;
using QSP.RouteFinding.Routes;
using QSP.FuelCalculation.Results;
using QSP.RouteFinding.Airports;
using QSP.FuelCalculation.FuelDataNew;
using static System.Math;
using QSP.RouteFinding.Data.Interfaces;
using static QSP.AviationTools.SpeedConversion;
using static QSP.AviationTools.ConversionTools;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Doubles;
using QSP.MathTools;
using QSP.MathTools.Vectors;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;

namespace QSP.FuelCalculation.Calculations
{
    // The first and last waypoints in route must be airports or runways.
    // The ident must start with the ICAO code of airport.
    /// <summary>
    /// Computes the actual fuel burn for the route.
    /// </summary>
    public class FuelCalculator
    {
        private const double deltaT = 0.5;    // Time in minute
        private AirportManager airportList;
        private CrzAltProvider altProvider;
        private IWindTableCollection windTable;
        private Route route;
        private FuelDataNew.FuelDataItem fuelData;

        public FuelCalculator(
            AirportManager airportList,
            CrzAltProvider altProvider,
            IWindTableCollection windTable,
            Route route,
            FuelDataNew.FuelDataItem fuelData)
        {
            if (route.Count < 2) throw new ArgumentException();

            this.airportList = airportList;
            this.altProvider = altProvider;
            this.windTable = windTable;
            this.route = route;
            this.fuelData = fuelData;
        }

        public DetailedPlan Calculate(
            double zfwKg,
            double landingFuelKg,
            double maxAltFt)
        {
            // We compute the flight backwards - from destination to origin.

            var planNodes = new List<PlanNode>();

            // Declare variables.
            LinkedListNode<RouteNode> node;
            Waypoint prevWpt;
            ICoordinate lastPt, currentPt;
            double grossWtKg, timeRemainMin, altFt, fuelOnBoardKg, optCrzAltFt,
                atcAllowedAltFt, targetAltFt, fuelFlowPerMinKg,
                descentGrad, timeToCrzAltMin, timeToNextWptMin, stepTimeMin,
                descentRateFtPerMin, stepDisNm, timeToCrzOrDelta, kias, ktas;
            bool isDescending;
            PlanNode lastPlanNode;

            // Initialize variables.           
            node = route.Last;
            prevWpt = node.Previous.Value.Waypoint;
            grossWtKg = zfwKg + landingFuelKg;
            timeRemainMin = 0.0;
            altFt = destElevationFt(route);
            fuelOnBoardKg = landingFuelKg;
            kias = fuelData.DescendKias;
            ktas = Ktas(kias, altFt);
            lastPlanNode = new PlanNode(node.Value, timeRemainMin,
                altFt, ktas, fuelOnBoardKg);
            lastPt = lastPlanNode.Coordinate;
            planNodes.Add(lastPlanNode);

            // Prepare the required parameters for the given step.
            optCrzAltFt = fuelData.OptCruiseAltFt(grossWtKg);
            atcAllowedAltFt = altProvider.ClosestAltitudeFt(
                lastPt, prevWpt, optCrzAltFt);
            targetAltFt = Min(atcAllowedAltFt, maxAltFt);
            isDescending = Abs(altFt - targetAltFt) > 0.1;

            if (isDescending)
            {
                fuelFlowPerMinKg = fuelData.DescentFuelPerMinKg(grossWtKg);
                descentGrad = fuelData.DescentGradient(grossWtKg);
                kias = fuelData.DescendKias;
                ktas = Ktas(kias, altFt);
                descentRateFtPerMin = descentGrad * ktas / 60.0 * NmFtRatio;
                timeToCrzAltMin = (targetAltFt - altFt) / descentRateFtPerMin;
            }
            else
            {
                fuelFlowPerMinKg = fuelData.CruiseFuelPerMinKg(grossWtKg);
                descentGrad = 0.0;
                kias = fuelData.CruiseKias(grossWtKg);
                ktas = Ktas(kias, altFt);
                descentRateFtPerMin = 0.0;
                timeToCrzAltMin = double.PositiveInfinity;
            }

            timeToNextWptMin = lastPt.Distance(prevWpt);
            timeToCrzOrDelta = Min(timeToCrzAltMin, deltaT);

            if (timeToNextWptMin <= timeToCrzOrDelta)
            {
                // Choose the next waypoint as current point.
                stepTimeMin = timeToNextWptMin;
                stepDisNm = stepTimeMin * ktas / 60.0;
                currentPt = prevWpt;

                // Update next waypoint.
                node = node.Previous;
                prevWpt = node.Value.Waypoint;
            }
            else
            {
                stepTimeMin = timeToCrzOrDelta;
                stepDisNm = stepTimeMin * ktas / 60.0;
                currentPt = GetV(lastPt, prevWpt, stepDisNm);
            }

            // Updating the value for the PlanNode.
            timeRemainMin += stepTimeMin;
            altFt += stepTimeMin * descentRateFtPerMin;
            fuelOnBoardKg += stepTimeMin * fuelFlowPerMinKg;

            // Add to flight plan.
            lastPlanNode = new PlanNode(new IntermediateNode(currentPt),
                timeRemainMin, altFt, ktas, fuelOnBoardKg);
            planNodes.Add(lastPlanNode);

            // Actually not. We are not done yet.
            if (prevWpt == null) return new DetailedPlan(planNodes);

            throw new NotImplementedException();
        }

        private double destElevationFt(Route route)
        {
            var icao = route.Last.Value.Waypoint.ID.Substring(0, 4).ToUpper();
            return airportList[icao].Elevation;
        }

        private static ICoordinate GetV(ICoordinate p1, ICoordinate p2,
            double disNm)
        {
            return EarthGeometry.GetV(
                p1.ToVector3D(),
                p2.ToVector3D(),
                disNm / EarthRadiusNm).ToLatLon();
        }
    }
}
