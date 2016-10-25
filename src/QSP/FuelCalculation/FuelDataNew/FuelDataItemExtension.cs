using QSP.MathTools.Interpolation;

namespace QSP.FuelCalculation.FuelDataNew
{
    public static class FuelDataItemExtension
    {
        public static double HoldingFuelKg(this FuelDataItem item,
            double timeMin, double weightTon)
        {
            return item.HoldingFuelPerMininuteKg(weightTon) * timeMin;
        }

        public static double HoldingFuelPerMininuteKg(this FuelDataItem item,
            double weightTon)
        {
            var refWt = item.HoldingFuelRefWtTon;
            var fuelFlow = item.HoldingFuelPerMinuteKg;
            return fuelFlow / refWt * weightTon;
        }

        public static double HoldingTimeMin(this FuelDataItem item,
            double fuelkg, double weightTon)
        {
            return fuelkg / item.HoldingFuelPerMininuteKg(weightTon);
        }

        public static double CruiseFuelPerMinKg(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.CruiseFuelPerMinKg, p2.CruiseFuelPerMinKg);
        }

        public static double CruiseKias(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.CruiseKias, p2.CruiseKias);
        }

        public static double ClimbGradient(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.ClimbGradient, p2.ClimbGradient);
        }

        public static double ClimbFuelPerMinKg(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.ClimbFuelPerMinKg, p2.ClimbFuelPerMinKg);
        }

        public static double DescentGradient(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.DescentGradient, p2.DescentGradient);
        }

        public static double DescentFuelPerMinKg(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.DescentFuelPerMinKg, p2.DescentFuelPerMinKg);
        }

        public static double OptCruiseAltFt(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.OptCruiseAltFt, p2.OptCruiseAltFt);
        }

        public static double EtopsCruiseKtas(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.EtopsCruiseKtas, p2.EtopsCruiseKtas);
        }

        public static double EtopsCruiseFuelPerMinKg(
            this FuelDataItem item, double grossWeightKg)
        {
            var p1 = item.DataPoint1;
            var p2 = item.DataPoint2;

            return Interpolate1D.Interpolate(
                p1.WeightKg, p2.WeightKg, grossWeightKg,
                p1.EtopsCruiseFuelPerMinKg, p2.EtopsCruiseFuelPerMinKg);
        }
    }
}
