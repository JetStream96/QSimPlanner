using CommonLibrary.LibraryExtension;
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
        // TODO: Add as an option?
        private static readonly double minContTime = 5.0;

        private readonly AirportManager airportList;
        private readonly ICrzAltProvider altProvider;
        private readonly IWindTableCollection windTable;
        private readonly Route routeToDest;
        private readonly IEnumerable<Route> routesToAltn;
        private readonly FuelParameters para;
        private readonly double maxAlt;

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
            this.routesToAltn = routesToAltn;
            this.para = para;
            this.maxAlt = maxAlt;
        }
        
        public FuelReport Generate()
        {
            var p = para;
            var f = p.FuelData;

            // Compute alternate part.
            var finalRsvFuel = f.HoldingFuelFlow(p.Zfw) * p.FinalRsvTime;
            var altnPlan = routesToAltn
                .Select(r => GetPlan(finalRsvFuel, r))
                .Select(d => d.AllNodes[0])
                .MaxBy(n => n.FuelOnBoard);
            var fuelToAltn = altnPlan.FuelOnBoard - finalRsvFuel;
            var timeToAltn = altnPlan.TimeRemaining;

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