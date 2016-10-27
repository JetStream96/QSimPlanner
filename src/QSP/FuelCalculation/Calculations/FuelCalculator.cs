using QSP.FuelCalculation.FuelDataNew;
using QSP.FuelCalculation.Results;
using QSP.LibraryExtension;
using QSP.MathTools;
using QSP.MathTools.Vectors;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using static QSP.AviationTools.Constants;
using static QSP.AviationTools.ConversionTools;
using static QSP.AviationTools.SpeedConversion;
using static QSP.MathTools.Doubles;
using static QSP.WindAloft.GroundSpeedCalculation;
using static System.Math;

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

            // ================ Declare variables ====================

            // Variable units:
            // Altitude: ft
            // Time: min
            // Distance: nm
            // Speed: knots
            // Climb/Descent rate: ft/min
            // Weight: kg
            // Fuel amount: kg
            // Fuel flow: kg/min

            LinkedListNode<RouteNode> node;
            Waypoint prevWpt;
            ICoordinate lastPt, currentPt;
            double grossWt, timeRemain, alt, fuelOnBoard, optCrzAlt,
                atcAllowedAlt, targetAlt, fuelFlow, descentGrad, timeToCrzAlt,
                timeToNextWpt, stepTime, descentRate, stepDis, 
                timeToCrzOrDelta, kias, ktas, gs;
            bool isDescending;
            PlanNode lastPlanNode;
            Vector3D v1, v2, v;

            // ================ Initialize variables ===================
            node = route.Last;
            prevWpt = node.Previous.Value.Waypoint;
            v1 = route.FirstWaypoint.ToVector3D();
            v2 = route.LastWaypoint.ToVector3D();
            v = prevWpt.ToVector3D();
            grossWt = zfwKg + landingFuelKg;
            timeRemain = 0.0;
            alt = destElevationFt(route);
            fuelOnBoard = landingFuelKg;
            kias = fuelData.DescendKias;
            ktas = Ktas(kias, alt);
            gs = GetGS(windTable, alt, ktas, v1, v2, v);
            lastPlanNode = new PlanNode(node.Value, timeRemain,
                alt, ktas, gs, fuelOnBoard);
            lastPt = lastPlanNode.Coordinate;
            planNodes.Add(lastPlanNode);

            while (prevWpt != null)
            {
                // Prepare the required parameters for the given step.
                optCrzAlt = fuelData.OptCruiseAltFt(grossWt);
                atcAllowedAlt = altProvider.ClosestAltitudeFt(
                    lastPt, prevWpt, optCrzAlt);
                targetAlt = Min(atcAllowedAlt, maxAltFt);
                isDescending = Abs(alt - targetAlt) > 0.1;

                if (isDescending)
                {
                    fuelFlow = fuelData.DescentFuelPerMinKg(grossWt);
                    descentGrad = fuelData.DescentGradient(grossWt);
                    kias = fuelData.DescendKias;
                    ktas = Ktas(kias, alt);
                    gs = GetGS(windTable, alt, ktas, v1, v2, v);
                    descentRate = descentGrad * ktas / 60.0 * NmFtRatio;
                    timeToCrzAlt = (targetAlt - alt) / descentRate;
                }
                else
                {
                    fuelFlow = fuelData.CruiseFuelPerMinKg(grossWt);
                    descentGrad = 0.0;
                    kias = fuelData.CruiseKias(grossWt);
                    ktas = Ktas(kias, alt);
                    gs = GetGS(windTable, alt, ktas, v1, v2, v);
                    descentRate = 0.0;
                    timeToCrzAlt = double.PositiveInfinity;
                }

                timeToNextWpt = lastPt.Distance(prevWpt) / gs * 60.0;
                timeToCrzOrDelta = Min(timeToCrzAlt, deltaT);

                if (timeToNextWpt <= timeToCrzOrDelta)
                {
                    // Choose the next waypoint as current point.
                    stepTime = timeToNextWpt;
                    stepDis = stepTime * ktas / 60.0;
                    currentPt = prevWpt;

                    // Update next waypoint.
                    node = node.Previous;
                    prevWpt = node.Value.Waypoint;
                }
                else
                {
                    stepTime = timeToCrzOrDelta;
                    stepDis = stepTime * ktas / 60.0;
                    currentPt = GetV(lastPt, prevWpt, stepDis);
                }

                // Updating the value for the PlanNode.
                timeRemain += stepTime;
                alt += stepTime * descentRate;
                fuelOnBoard += stepTime * fuelFlow;

                // Add to flight plan.
                lastPlanNode = new PlanNode(new IntermediateNode(currentPt),
                    timeRemain, alt, ktas, gs, fuelOnBoard);
                planNodes.Add(lastPlanNode);
            }

            // Actually not. We are not done yet.
            return new DetailedPlan(planNodes);

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
