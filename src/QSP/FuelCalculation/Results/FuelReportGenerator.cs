using QSP.LibraryExtension;
using QSP.FuelCalculation.Calculations;
using QSP.FuelCalculation.Calculations.Corrections;
using QSP.FuelCalculation.FuelData;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.FuelCalculation.Results
{
    // The units of variables used in this class is specified in 
    // VariableUnitStandard.txt.

    public class FuelReportGenerator
    {
        // Minimum time for contingency fuel.
        private static readonly double minContTime = 5.0;

        private readonly AirportManager airportList;
        private readonly ICrzAltProvider altProvider;
        private readonly IWindTableCollection windTable;
        private readonly Route routeToDest;
        private readonly IReadOnlyList<Route> routesToAltn;
        private readonly FuelParameters para;
        private readonly double maxAlt;

        /// <summary>
        /// Note: routesToAltn can be empty. In that case, alternates are not used.
        /// </summary>
        public FuelReportGenerator(
            AirportManager airportList,
            ICrzAltProvider altProvider,
            IWindTableCollection windTable,
            Route routeToDest,
            IEnumerable<Route> routesToAltn,
            FuelParameters para,
            double maxAlt = 41000.0)
        {
            this.airportList = airportList;
            this.altProvider = altProvider;
            this.windTable = windTable;
            this.routeToDest = routeToDest;
            this.routesToAltn = routesToAltn.ToList();
            this.para = para;
            this.maxAlt = maxAlt;
        }

        /// <exception cref="ElevationDifferenceTooLargeException"></exception>
        /// <exception cref="Exception"></exception>
        public FuelReport Generate()
        {
            var p = para;
            var f = p.FuelData;

            // Final reserve
            var finalRsvFuel = f.HoldingFuelFlow(p.Zfw) * p.FinalRsvTime;

            // Compute alternate part. Alternates are optional under some circumstances.
            // See https://aviation.stackexchange.com/questions/389/is-an-alternate-airport-always-required-when-flying-internationally
            var (fuelToAltn, timeToAltn) = (0.0, 0.0);
            if (routesToAltn.Count > 0)
            {
                var altnPlan = routesToAltn
                               .Select(r => GetPlan(finalRsvFuel, r))
                               .Select(d => d.AllNodes[0])
                               .MaxBy(n => n.FuelOnBoard);
                fuelToAltn = altnPlan.FuelOnBoard - finalRsvFuel;
                timeToAltn = altnPlan.TimeRemaining;
            }

            // Destination part.
            var fuelHold = f.HoldingFuelFlow * p.HoldingTime;
            var timeExtra = p.ExtraFuel / f.HoldingFuelFlow;
            var destLandingFuel = fuelToAltn + fuelHold + p.ExtraFuel + finalRsvFuel;
            var destPlan = GetPlan(destLandingFuel, routeToDest).AllNodes[0];
            var fuelToDest = destPlan.FuelOnBoard - destLandingFuel;
            var timeToDest = destPlan.TimeRemaining;
            var timeCont = Math.Max(minContTime, timeToDest * p.ContPercent / 100.0);
            var contFuel = fuelToDest * timeCont / timeToDest;

            return new FuelReport(
                fuelToDest,
                fuelToAltn,
                contFuel,
                p.ExtraFuel,
                fuelHold,
                p.ApuTime * f.ApuFuelFlow,
                p.TaxiTime * f.TaxiFuelFlow,
                finalRsvFuel,
                timeToDest,
                timeToAltn,
                timeCont,
                timeExtra,
                p.HoldingTime,
                p.FinalRsvTime,
                p.ApuTime,
                p.TaxiTime);
        }

        /// <exception cref="ElevationDifferenceTooLargeException"></exception>
        /// <exception cref="Exception"></exception>
        private DetailedPlan GetPlan(double landingFuel, Route r)
        {
            return new FuelCalculator(
                airportList,
                altProvider,
                windTable,
                r,
                para.FuelData,
                para.Zfw,
                landingFuel,
                maxAlt).Create().ApplyCorrection(para.FuelData);
        }
    }
}
