using QSP.LibraryExtension;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.FuelCalculation.Calculators
{
    /*
    // Bias is applied in this calculator.
    public class FuelCalculatorWithWind
    {
        private FuelDataItem fuelData;
        private FuelParameters para;
        private IWindTableCollection windTables;

        public FuelCalculatorWithWind(
            FuelDataItem fuelData,
            FuelParameters para,
            IWindTableCollection windTables)
        {
            this.fuelData = fuelData;
            this.para = para;
            this.windTables = windTables;
        }

        public FuelReport Compute(Route routeToDest,
            IEnumerable<Route> routesToAltn)
        {
            var altnAirDis = routesToAltn.Select(r =>
            GetAirDisNm(r, 1, null));

            var altnResults = altnAirDis.Select(dis =>
            {
                var altnCalc = new AlternateFuelCalculator(fuelData, para);
                return altnCalc.Compute(dis);
            });

            var maxAltnFuelResult = altnResults.MaxBy(r => r.FuelTon);
            var destAirDis = GetAirDisNm(routeToDest, 1, maxAltnFuelResult);
            var destCalc = new DestinationFuelCalculator(
                fuelData, para, maxAltnFuelResult);
            var destResult = destCalc.Compute(destAirDis);
            var extraTimeMin = fuelData.HoldingTimeMin(
                para.ExtraFuelKg / para.FuelBias,
                destResult.LandingWeightTon);
            var contingencyKg = destResult.FuelTon * 1000.0
                * para.ContPercent / 100.0
                * para.FuelBias;

            return new FuelReport(
                destResult.FuelTon,
                maxAltnFuelResult.FuelTon,
                contingencyKg,
                para.ExtraFuelKg,
                destResult.HoldingFuelKg,                
                para.ApuTimeMin * fuelData.ApuFuelPerMinKg,
                para.TaxiTimeMin * fuelData.TaxiFuelPerMinKg * para.FuelBias,
                altnResults.First().HoldingFuelKg,
                destResult.TimeMin,
                maxAltnFuelResult.TimeMin,
                extraTimeMin,
                para.HoldingMin,
                para.FinalRsvMin,
                para.ApuTimeMin,
                para.TaxiTimeMin);
        }

        // Compute air distance iteratively.
        // The reason to do this iteratively is that
        // the amount of fuel required depends on wind,
        // but wind depends on cruise altitude, which depends
        // on weight of aircraft, which depends on fuel required.
        //
        // iterationCount should be non-negative.
        // Smaller num = less precise, although 1 is usually good enough.
        //
        // If calculating air for a route to alternate, leave alternateInfo
        // null. Otherwise, i.e. to destination, pass the correct variable.
        //
        private double GetAirDisNm(
            Route route,
            int iterationCount,
            CalculationResult alternateInfo = null)
        {
            var optCrz = fuelData.OptCrzTable;
            var speedProfile = fuelData.SpeedProfile;
            var calc = AirDisToResultMap(alternateInfo);

            double fuelTon = 0.0;
            double avgWeightTon = 0.0;
            double crzAltFt = 0.0;
            double tas = 0.0;
            double airDis = route.TotalDistance();

            for (uint i = 0; i <= iterationCount; i++)
            {
                var result = calc(airDis);
                fuelTon = result.FuelTon;
                avgWeightTon = result.LandingWeightTon + fuelTon / 2.0;
                crzAltFt = optCrz.ActualCrzAltFt(avgWeightTon, airDis);
                tas = speedProfile.CruiseTasKnots(crzAltFt);
                airDis = ComputeAirDistance(route, tas, crzAltFt);
            }

            return airDis;
        }

        private Func<double, CalculationResult> AirDisToResultMap(
            CalculationResult alternateInfo)
        {
            if (alternateInfo == null)
            {
                return _airDis =>
                new AlternateFuelCalculator(fuelData, para)
                .Compute(_airDis);
            }
            else
            {
                return _airDis =>
                new DestinationFuelCalculator(fuelData, para, alternateInfo)
                .Compute(_airDis);
            }
        }

        private double ComputeAirDistance(
            Route route, double tas, double cruiseAltitudeFt)
        {
            return new AvgWindCalculator(windTables, tas, cruiseAltitudeFt)
                .GetAirDistance(route.Select(n => n.Waypoint));
        }
    }*/
}
