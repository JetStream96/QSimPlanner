using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;
using QSP.FuelCalculation.Results;
using static QSP.AviationTools.Heading.HeadingCalculation;
using static QSP.FuelCalculation.Calculations.InitialPlanCreator;

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
        private readonly FuelDataNew.FuelDataItem fuelData;
        private readonly double zfw;
        private readonly double landingFuel;
        private readonly double maxAlt;

        public FuelCalculator(
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

        public DetailedPlan Create()
        {
            var initPlan = new InitialPlanCreator(
                airportList,
                altProvider,
                windTable,
                route,
                fuelData,
                zfw,
                landingFuel,
                maxAlt).Create();

            var climbNodes = new ClimbNodesCreator(
                airportList, route, fuelData, initPlan).Create();

            var completeRoute = climbNodes
                .Concat(initPlan.Skip(climbNodes.Count))
                .ToList();


        }

        private List<PlanNode> TrimCruiseAlt(List<PlanNode> nodes)
        {
            int tocIndex = GetTocIndex(nodes);
            if (ReachedCruisingAlt(nodes, tocIndex)) return nodes;

            var alt = altProvider.ClosestAltBelow(
                nodes[tocIndex-1])
        }

        private static int GetTocIndex(IReadOnlyList<PlanNode> nodes)
        {
            for (int i = 0; i < nodes.Count-1; i++)
            {
                if (nodes[i].Alt + AltDiffCriteria >= nodes[i + 1].Alt)
                {
                    return i;
                }
            }

            throw new ArgumentException();
        }

        private bool ReachedCruisingAlt(IReadOnlyList<PlanNode> n, int tocIndex)
        {
            var toc = n[tocIndex];
            var heading = Heading(toc, n[tocIndex + 1]);
            return altProvider.IsValidCrzAlt(toc, heading, toc.Alt);
        }
    }
}
