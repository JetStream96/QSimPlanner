using QSP.FuelCalculation.FuelData;
using QSP.FuelCalculation.Results;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Heading.HeadingCalculation;
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

        // May throw exception.
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
        
        /// <exception cref="InvalidPlanAltitudeException"></exception>
        public DetailedPlan Create()
        {
            var altLimit = maxAlt;

            while (true)
            {
                var plan = Mark(GetPlan(altLimit));
                var altResult = CruiseAltValid(plan);
                if (altResult.IsValid) return new DetailedPlan(plan);
                var minAlt = Math.Max(plan.First().Alt, plan.Last().Alt);
                altLimit = altResult.NewAlt;
                if (altLimit <= minAlt) throw new InvalidPlanAltitudeException();
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

            var climbNodes = new ClimbNodesCreator(
                airportList, route, fuelData, initPlan).Create();

            return climbNodes
                .Concat(initPlan.Skip(climbNodes.Count))
                .ToList();
        }

        private AltResult CruiseAltValid(IReadOnlyList<IPlanNode> nodes)
        {
            int tocIndex = TocIndex(nodes);
            var toc = nodes[tocIndex];
            var heading = Heading(toc, nodes[tocIndex + 1]);
            bool valid = altProvider.IsValidCrzAlt(toc, heading, toc.Alt);
            double newAlt = valid ? toc.Alt :
                altProvider.ClosestAltBelow(toc, heading, toc.Alt);
            return new AltResult() { IsValid = valid, NewAlt = newAlt };
        }

        private struct AltResult
        {
            public bool IsValid;
            public double NewAlt;
        }
    }
}
