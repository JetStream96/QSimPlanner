using System.Collections.Generic;
using QSP.Common.Options;
using QSP.FuelCalculation.Calculations;
using QSP.FuelCalculation.FuelData;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Data.Interfaces;
using QSP.Common;
using static QSP.AviationTools.SpeedConversion;

namespace QSP.WindAloft
{
    public static class AvgWindCalculatorExtension
    {
        public static double GetAirDistance(
            this AvgWindCalculator calc, IEnumerable<ICoordinate> route)
        {
            double airDis = 0.0;
            ICoordinate last = null;

            foreach (var i in route)
            {
                if (last != null) airDis += calc.GetAirDistance(last, i);
                last = i;
            }

            return airDis;
        }

        /// <summary>
        /// Get AvgWindCalculator to approximate the wind.
        /// Returns null if user disabled wind optimization.
        /// </summary>
        /// <exception cref="InvalidUserInputException"></exception>
        public static AvgWindCalculator GetWindCalculator(
            AppOptions appSettings,
            Locator<IWxTableCollection> windTableLocator,
            AirportManager airportList,
            FuelDataItem fuelData,
            double zfwTon,
            string orig,
            string dest)
        {
            if (!appSettings.EnableWindOptimizedRoute) return null;

            if (windTableLocator.Instance is DefaultWxTableCollection)
            {
                throw new InvalidUserInputException(
                    "Wind data has not been downloaded or loaded from file.\n" +
                    "If you do not want to use wind-optimized route, it can be disabled " +
                    "from Options > Route.");
            }

            if (fuelData == null)
            {
                throw new InvalidUserInputException("No aircraft is selected.");
            }

            var origin = airportList[orig.Trim().ToUpper()];

            if (orig == null)
            {
                throw new InvalidUserInputException("Cannot find origin airport.");
            }

            var destination = airportList[dest.ToUpper()];

            if (dest == null)
            {
                throw new InvalidUserInputException("Cannot find destination airport.");
            }

            var dis = origin.Distance(destination);
            var alt = fuelData.EstimatedCrzAlt(dis, zfwTon);
            var tas = Ktas(fuelData.CruiseKias(zfwTon), alt);

            return new AvgWindCalculator(windTableLocator.Instance, tas, alt);
        }
    }
}
