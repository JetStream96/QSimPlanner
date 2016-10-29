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
using QSP.Common;
using static QSP.AviationTools.Constants;
using static QSP.AviationTools.SpeedConversion;
using static QSP.MathTools.Doubles;
using static QSP.WindAloft.GroundSpeedCalculation;
using static System.Math;
using QSP.FuelCalculation.Results.Nodes;

namespace QSP.FuelCalculation.Calculations
{
    // The first and last waypoints in route must be airports or runways.
    // The ident must start with the ICAO code of airport.
    //
    // The units of variables used in this class is specified in 
    // VariableUnitStandard.txt.

    /// <summary>
    /// Computes the actual fuel burn for the route.
    /// </summary>
    public class FuelCalculator
    {
        private readonly double deltaT = 0.5;    // Time in minute
        private readonly AirportManager airportList;
        private readonly CrzAltProvider altProvider;
        private readonly IWindTableCollection windTable;
        private readonly Route route;
        private readonly FuelDataNew.FuelDataItem fuelData;

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
            
            LinkedListNode<RouteNode> node;
            Waypoint prevWpt;
            ICoordinate prevCoord, currentCoord;
            double grossWt, timeRemain, alt, fuelOnBoard, optCrzAlt,
                atcAllowedAlt, targetAlt, fuelFlow, descentGrad, timeToCrzAlt,
                timeToNextWpt, stepTime, descentRate, stepDis, kias, ktas, gs;
            bool isDescending;
            PlanNode prevPlanNode;
            Vector3D v1, v2, v;

            // ================ Initialize variables ===================
            node = route.Last;
            prevWpt = node.Previous.Value.Waypoint;
            v1 = route.FirstWaypoint.ToVector3D();
            v2 = route.LastWaypoint.ToVector3D();
            v = v2;
            grossWt = zfwKg + landingFuelKg;
            timeRemain = 0.0;
            alt = DestElevationFt();
            fuelOnBoard = landingFuelKg;
            kias = fuelData.DescendKias;
            ktas = Ktas(kias, alt);
            gs = GetGS(windTable, alt, ktas, v1, v2, v);
            prevPlanNode = new PlanNode(node.Value, timeRemain,
                alt, ktas, gs, fuelOnBoard);
            prevCoord = prevPlanNode.Coordinate;
            planNodes.Add(prevPlanNode);

            while (prevWpt != null)
            {
                // Continue the loop if we have not reached origin airport.

                // Prepare the required parameters for the given step.
                grossWt = prevPlanNode.FuelOnBoardKg + zfwKg;
                optCrzAlt = fuelData.OptCruiseAltFt(grossWt);
                atcAllowedAlt = altProvider.ClosestAltitudeFtFrom(
                    prevWpt, prevCoord, optCrzAlt);
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

                timeToNextWpt = prevCoord.Distance(prevWpt) / gs * 60.0;

                double[] times = { deltaT, timeToCrzAlt, timeToNextWpt };
                int minIndex = times.MinIndex();
                stepTime = times[minIndex];
                stepDis = stepTime * ktas / 60.0;

                // Node to add to flight plan.
                object nodeVal = null;

                switch (minIndex)
                {
                    case 0:
                        currentCoord = GetV(prevCoord, prevWpt, stepDis);
                        nodeVal = new IntermediateNode(currentCoord);
                        break;

                    case 1:
                        currentCoord = GetV(prevCoord, prevWpt, stepDis);
                        nodeVal = new TodNode(currentCoord);
                        break;

                    case 2:
                        // Choose the next waypoint as current point.
                        currentCoord = prevWpt;
                        nodeVal = node.Previous.Value;

                        // Update next waypoint.
                        node = node.Previous;
                        prevWpt = node.Value.Waypoint;
                        break;

                    default:
                        throw new UnexpectedExecutionStateException();
                }

                // Updating the value for the PlanNode.
                timeRemain += stepTime;
                alt += stepTime * descentRate;
                fuelOnBoard += stepTime * fuelFlow;

                prevPlanNode = new PlanNode(nodeVal,
                    timeRemain, alt, ktas, gs, fuelOnBoard);
                prevCoord = prevPlanNode.Coordinate;
                v = prevCoord.ToVector3D();
                planNodes.Add(prevPlanNode);
            }

            planNodes.Reverse();

            // Now the planNodes contains all nodes from destination to origin,
            // but the profile is flying from origin to TOD at cruising 
            // altitude and then descend to destination.
            // We need to calculate the climb part.

            // Use the gross weight at origin already computed. It is not 
            // correct but close enough as an approximation. Similarly, the
            // fuelOnBoard and TimeRemaining are approximations only.

            var climbNodes = new List<PlanNode>();

            double climbGrad, climbRate;
            Waypoint nextWpt;
            bool isClimbing;

            node = route.First;
            nextWpt = node.Next.Value.Waypoint;
            v = v1;
            timeRemain = 0.0;
            fuelOnBoard = planNodes[0].FuelOnBoardKg;
            grossWt = zfwKg + fuelOnBoard;
            alt = OrigElevationFt();
            kias = fuelData.ClimbKias;
            ktas = Ktas(kias, alt);
            gs = GetGS(windTable, alt, ktas, v1, v2, v);
            prevPlanNode = new PlanNode(node.Value, timeRemain,
                alt, ktas, gs, fuelOnBoard);
            prevCoord = prevPlanNode.Coordinate;
            planNodes.Add(prevPlanNode);

            while (true) //TODO:what is the right condition?
            {
                // Prepare the required parameters for the given step.
                grossWt = prevPlanNode.FuelOnBoardKg + zfwKg;
                optCrzAlt = fuelData.OptCruiseAltFt(grossWt);
                atcAllowedAlt = altProvider.ClosestAltitudeFtTo(
                    prevCoord, nextWpt, optCrzAlt);
                targetAlt = Min(atcAllowedAlt, maxAltFt);
                isClimbing = Abs(alt - targetAlt) > 0.1;

                if (isClimbing)
                {
                    fuelFlow = fuelData.ClimbFuelPerMinKg(grossWt);
                    climbGrad = fuelData.ClimbGradient(grossWt);
                    kias = fuelData.ClimbKias;
                    ktas = Ktas(kias, alt);
                    gs = GetGS(windTable, alt, ktas, v1, v2, v);
                    climbRate = climbGrad * ktas / 60.0 * NmFtRatio;
                    timeToCrzAlt = (targetAlt - alt) / climbRate;
                }
                else
                {
                    fuelFlow = fuelData.CruiseFuelPerMinKg(grossWt);
                    descentGrad = 0.0;
                    kias = fuelData.CruiseKias(grossWt);
                    ktas = Ktas(kias, alt);
                    gs = GetGS(windTable, alt, ktas, v1, v2, v);
                    climbRate = 0.0;
                    timeToCrzAlt = double.PositiveInfinity;
                }

                timeToNextWpt = prevCoord.Distance(nextWpt) / gs * 60.0;

                double[] times = { deltaT, timeToCrzAlt, timeToNextWpt };
                int minIndex = times.MinIndex();
                stepTime = times[minIndex];
                stepDis = stepTime * ktas / 60.0;

                // Node to add to flight plan.
                object nodeVal = null;

                switch (minIndex)
                {
                    case 0:
                        currentCoord = GetV(prevCoord, nextWpt, stepDis);
                        nodeVal = new IntermediateNode(currentCoord);
                        break;

                    case 1:
                        currentCoord = GetV(prevCoord, nextWpt, stepDis);
                        nodeVal = new TocNode(currentCoord);
                        break;

                    case 2:
                        // Choose the next waypoint as current point.
                        currentCoord = nextWpt;
                        nodeVal = node.Next.Value;

                        // Update next waypoint.
                        node = node.Next;
                        nextWpt = node.Value.Waypoint;
                        break;

                    default:
                        throw new UnexpectedExecutionStateException();
                }

                // Updating the value for the PlanNode.
                timeRemain -= stepTime;
                alt += stepTime * climbRate;
                fuelOnBoard -= stepTime * fuelFlow;

                prevPlanNode = new PlanNode(nodeVal,
                    timeRemain, alt, ktas, gs, fuelOnBoard);
                prevCoord = prevPlanNode.Coordinate;
                v = prevCoord.ToVector3D();
                planNodes.Add(prevPlanNode);
            }

            // Actually not. We are not done yet.
            return new DetailedPlan(planNodes);

            throw new NotImplementedException();
        }
        
        private double OrigElevationFt()
        {
            var icao = route.First.Value.Waypoint.ID.Substring(0, 4).ToUpper();
            return airportList[icao].Elevation;
        }

        private double DestElevationFt()
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
