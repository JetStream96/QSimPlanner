using System;
using QSP.MathTools.Interpolation;

namespace QSP.FuelCalculation.FuelData
{
    // The units of variables used in this class is specified in 
    // FuelCalculation/Calculations/VariableUnitStandard.txt.

    public static class FuelDataItemExtension
    {
        public static double HoldingFuel(this FuelDataItem item, double time, double weight)
        {
            return item.HoldingFuelFlow(weight) * time;
        }

        public static double HoldingFuelFlow(this FuelDataItem item, double weight)
        {
            var refWt = item.HoldingFuelRefWt;
            var fuelFlow = item.HoldingFuelFlow;
            return fuelFlow / refWt * weight;
        }

        public static double HoldingTime(this FuelDataItem item, double fuel, double weight)
        {
            return fuel / item.HoldingFuelFlow(weight);
        }

        public static double CruiseFuelFlow(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.CruiseFuelFlow, grossWeight);
        }

        public static double CruiseKias(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.CruiseKias, grossWeight);
        }

        public static double ClimbGradient(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.ClimbGradient, grossWeight);
        }

        public static double ClimbFuelFlow(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.ClimbFuelFlow, grossWeight);
        }

        public static double DescentGradient(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.DescentGradient, grossWeight);
        }

        public static double DescentFuelFlow(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.DescentFuelFlow, grossWeight);
        }

        public static double OptCruiseAlt(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.OptCruiseAlt, grossWeight);
        }

        public static double EtopsCruiseKtas(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.EtopsCruiseKtas, grossWeight);
        }

        public static double EtopsCruiseFuelFlow(this FuelDataItem item, double grossWeight)
        {
            return InterpolateHelper(item, p => p.EtopsCruiseFuelFlow, grossWeight);
        }

        private static double InterpolateHelper(FuelDataItem item,
            Func<DataPoint, double> getter, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, getter(p1), getter(p2), grossWeight);
        }

        /// <summary>
        /// Apply fuel bias to the given FuelDataItem. If bias is 1 then 
        /// return value will be the same as original.
        /// </summary>
        public static FuelDataItem WithBias(this FuelDataItem f, double bias)
        {
            return new FuelDataItem(
                f.HoldingFuelFlow * bias,
                f.HoldingFuelRefWt,
                f.TaxiFuelFlow * bias,
                f.ApuFuelFlow,
                f.MissedAppFuel * bias,
                f.ClimbKias,
                f.DescendKias,
                f.DataPoint1.WithBias(bias),
                f.DataPoint2.WithBias(bias),
                f.FuelTable);
        }
    }
}
