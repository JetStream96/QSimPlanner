using QSP.MathTools.Interpolation;

namespace QSP.FuelCalculation.FuelDataNew
{
    // The units of variables used in this class is specified in 
    // FuelCalculation/Calculations/VariableUnitStandard.txt.

    public static class FuelDataItemExtension
    {
        public static double HoldingFuel(this FuelDataItem item,
            double time, double weight)
        {
            return item.HoldingFuelFlow(weight) * time;
        }

        public static double HoldingFuelFlow(this FuelDataItem item,
            double weight)
        {
            var refWt = item.HoldingFuelRefWt;
            var fuelFlow = item.HoldingFuelFlow;
            return fuelFlow / refWt * weight;
        }

        public static double HoldingTime(this FuelDataItem item,
            double fuel, double weight)
        {
            return fuel / item.HoldingFuelFlow(weight);
        }

        public static double CruiseFuelFlow(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.CruiseFuelFlow, p2.CruiseFuelFlow, grossWeight);
        }

        public static double CruiseKias(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.CruiseKias, p2.CruiseKias, grossWeight);
        }

        public static double ClimbGradient(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.ClimbGradient, p2.ClimbGradient, grossWeight);
        }

        public static double ClimbFuelFlow(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.ClimbFuelFlow, p2.ClimbFuelFlow, grossWeight);
        }

        public static double DescentGradient(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.DescentGradient, p2.DescentGradient, grossWeight);
        }

        public static double DescentFuelFlow(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.DescentFuelFlow, p2.DescentFuelFlow, grossWeight);
        }

        public static double OptCruiseAlt(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.OptCruiseAlt, p2.OptCruiseAlt, grossWeight);
        }

        public static double EtopsCruiseKtas(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.EtopsCruiseKtas, p2.EtopsCruiseKtas, grossWeight);
        }

        public static double EtopsCruiseFuelFlow(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight,
                p1.EtopsCruiseFuelFlow, p2.EtopsCruiseFuelFlow, grossWeight);
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
                f.DataPoint2.WithBias(bias));
        }

        public static double EstimatedCrzAlt(this FuelDataItem item,
            double distance, double landingFuel)
        {

        }
    }
}
