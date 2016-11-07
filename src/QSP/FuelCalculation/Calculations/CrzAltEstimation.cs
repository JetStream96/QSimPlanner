using System;
using QSP.FuelCalculation.FuelData;
using static QSP.AviationTools.Constants;
using static QSP.AviationTools.SpeedConversion;

namespace QSP.FuelCalculation.Calculations
{
    // The units of variables used in this class is specified in 
    // VariableUnitStandard.txt.

    public static class CrzAltEstimation
    {
        public static double EstimatedCrzAlt(this FuelDataItem item,
            double distance, double landingWt)
        {
            var grossWt = item.EstimatedAvgWt(distance, landingWt);
            var lim = item.DistanceLimitedAltitude(distance, grossWt);
            var opt = item.OptCruiseAlt(grossWt);
            return Math.Min(lim, opt);
        }

        public static double EstimatedAvgWt(this FuelDataItem item,
            double distance, double landingWt)
        {
            double grossWt = landingWt;
            const int iteration = 2;

            for (int i = 0; i < iteration; i++)
            {
                var alt = item.OptCruiseAlt(grossWt);
                var ktas = Ktas(item.CruiseKias(grossWt), alt);
                var time = distance / ktas * 60.0;
                var ff = item.CruiseFuelFlow(grossWt);
                grossWt = landingWt + ff * time / 2.0;
            }

            return grossWt;
        }

        public static double DistanceLimitedAltitude(this FuelDataItem item,
            double distance, double grossWt)
        {
            var c = item.ClimbGradient(grossWt);
            var d = item.DescentGradient(grossWt);
            return distance * NmFtRatio * c * d / (c + d);
        }
    }
}