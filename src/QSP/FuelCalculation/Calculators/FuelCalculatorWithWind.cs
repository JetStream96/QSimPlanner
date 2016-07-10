using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System;
using static QSP.WindAloft.Utilities;

namespace QSP.FuelCalculation.Calculators
{
    public class FuelCalculatorWithWind
    {
        private FuelDataItem fuelData;
        private FuelParameters para;
        private AirDistanceCollection airDis;
        private WindTableCollection windTables;

        public FuelCalculatorWithWind(
            FuelDataItem fuelData,
            FuelParameters para,
            AirDistanceCollection airDis,
            WindTableCollection windTables)
        {
            this.fuelData = fuelData;
            this.para = para;
            this.airDis = airDis;
            this.windTables = windTables;
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
        // If calculating air for a route to destination, leave 
        // alternateInfo null. Otherwise, i.e. to alternate, pass the correct
        // variable.
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
            double airDis = route.GetTotalDistance();

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
                new DestinationFuelCalculator(fuelData, para, alternateInfo)
                .Compute(_airDis);
            }
            else
            {
                return _airDis =>
                new AlternateFuelCalculator(fuelData, para)
                .Compute(_airDis);
            }
        }

        private double ComputeAirDistance(
            Route route, double tas, double cruiseAltitudeFt)
        {
            return GetAirDistance(windTables, route, cruiseAltitudeFt, tas);
        }
    }
}
