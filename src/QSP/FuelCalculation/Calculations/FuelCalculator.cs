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
            // Initialize variables.
            var waypoints = new List<PlanNode>();

            var node = route.Last;
            var wpt = node.Value.Waypoint;
            var nextWpt = node.Next.Value.Waypoint;
            ICoordinate nextPt;
            double grossWtKg = zfwKg + landingFuelKg;
            double timeRemainMin = 0.0;
            var destIcao = wpt.ID.Substring(0, 4).ToUpper();
            double altFt = airportList[destIcao].Elevation;
            double fuelOnBoardKg = landingFuelKg;
            double optCrzAltFt, atcAllowedAltFt, targetAltFt, fuelFlowPerMinKg,
                descentGrad, timeToCrzAltMin, timeToNextWptMin, stepTimeMin,
                descentRateFtPerMin, stepDisNm;
            double kias = fuelData.DescendKias;
            double ktas = Ktas(kias, altFt);
            bool isDescending;
            var lastPlanNode = new PlanNode(node.Value, timeRemainMin,
                altFt, ktas, fuelOnBoardKg);
            waypoints.Add(lastPlanNode);

            // Do computations.
            optCrzAltFt = fuelData.OptCruiseAltFt(grossWtKg);
            atcAllowedAltFt = altProvider.ClosestAltitudeFt(
                wpt, nextWpt, optCrzAltFt);
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

            timeToNextWptMin = wpt.Distance(nextWpt);
            stepTimeMin = Min(timeToNextWptMin, timeToCrzAltMin, deltaT);
            stepDisNm = stepTimeMin * ktas / 60.0;
            nextPt = GetV(lastPlanNode.Coordinate, nextWpt, stepDisNm);

            waypoints.Add(new PlanNode(node.Value, ))

            throw new NotImplementedException();
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
