using QSP.FuelCalculation.FuelData;
using QSP.FuelCalculation.Results;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.FuelCalculation.Calculations.NodeMarker;

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
        private readonly AirportManager airportList;
        private readonly ICrzAltProvider altProvider;
        private readonly IWindTableCollection windTable;
        private readonly Route route;
        private readonly FuelDataItem fuelData;
        private readonly double zfw;
        private readonly double landingFuel;
        private readonly double maxAlt;

        /// <exception cref="ArgumentException"></exception>
        public FuelCalculator(
            AirportManager airportList,
            ICrzAltProvider altProvider,
            IWindTableCollection windTable,
            Route route,
            FuelDataItem fuelData,
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

        public DetailedPlan Create()
        {
            var altLimit = maxAlt;

            // Try to find a valid cruising altitude.
            // The GetPlan method returns a complete route with altitudes.
            // However, this route is based on altLimit, e.g. 41000 feet.
            // This may not be a valid cruising altitude, e.g. for westbound flights.
            //
            // In the above case, the next valid altitude below is used as altLimit for
            // next iteration, i.e. 40000 feet. This does not alway work since on
            // very short routes (e.g. 10 NM), the next valid altitude below may be
            // lower than airport elevation. In this case, we do not attempt to find
            // another altitude.
            //
            // This method does not handle the cases such as when flight turns 
            // from westbound to eastbound. But overall this is a good approximation
            // for fuel estimation purpose.
            //
            while (true)
            {
                var plan = Mark(GetPlan(altLimit));
                var (altValid, newAlt) = CalculationUtil.CruiseAltValid(altProvider, plan);
                if (altValid) return new DetailedPlan(plan);
                var minAlt = Math.Max(plan.First().Alt, plan.Last().Alt);
                altLimit = newAlt;
                if (altLimit <= minAlt) return new DetailedPlan(plan);
            }
        }

        private List<IPlanNode> GetPlan(double altLimit)
        {
            var initPlan = new InitialPlanCreator(
                  airportList,
                  altProvider,
                  windTable,
                  route,
                  fuelData,
                  zfw,
                  landingFuel,
                  altLimit).Create();

            var climbCreator = new ClimbNodesCreator(airportList, route, fuelData, initPlan);

            try
            {
                var climbNodes = climbCreator.Create();

                return climbNodes
                    .Concat(initPlan.Skip(climbNodes.Count))
                    .ToList();
            }
            catch (ElevationDifferenceTooLargeException e)
            {
                // TODO: what to do here?
                throw;
            }
        }
    }
}
