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
                p1.Weight, p2.Weight, grossWeight,
                p1.CruiseFuelFlow, p2.CruiseFuelFlow);
        }

        public static double CruiseKias(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, grossWeight,
                p1.CruiseKias, p2.CruiseKias);
        }

        public static double ClimbGradient(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, grossWeight,
                p1.ClimbGradient, p2.ClimbGradient);
        }

        public static double ClimbFuelFlow(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, grossWeight,
                p1.ClimbFuelFlow, p2.ClimbFuelFlow);
        }

        public static double DescentGradient(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, grossWeight,
                p1.DescentGradient, p2.DescentGradient);
        }

        public static double DescentFuelFlow(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, grossWeight,
                p1.DescentFuelFlow, p2.DescentFuelFlow);
        }

        public static double OptCruiseAlt(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, grossWeight,
                p1.OptCruiseAlt, p2.OptCruiseAlt);
        }

        public static double EtopsCruiseKtas(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, grossWeight,
                p1.EtopsCruiseKtas, p2.EtopsCruiseKtas);
        }

        public static double EtopsCruiseFuelFlow(
            this FuelDataItem item, double grossWeight)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.Weight, p2.Weight, grossWeight,
                p1.EtopsCruiseFuelFlow, p2.EtopsCruiseFuelFlow);
        }
    }
}
