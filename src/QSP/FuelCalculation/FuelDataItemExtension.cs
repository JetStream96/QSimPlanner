namespace QSP.FuelCalculation
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
    }
}
