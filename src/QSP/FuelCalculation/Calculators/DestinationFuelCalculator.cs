namespace QSP.FuelCalculation.Calculators
{
    public class DestinationFuelCalculator
    {
        private FuelDataItem fuelData;
        private FuelParameters para;
        private CalculationResult altnResult;

        public DestinationFuelCalculator(
            FuelDataItem fuelData,
            FuelParameters para,
            CalculationResult altnResult)
        {
            this.fuelData = fuelData;
            this.para = para;
            this.altnResult = altnResult;
        }

        public CalculationResult Compute(double airDistanceNm)
        {
            double landingWeightTon = LandWeightTon();
            double timeMin =
                fuelData.FlightTimeTable.GetTimeMin(airDistanceNm);

            double fuelTon = fuelData.FuelTable.GetFuelRequiredTon(
                airDistanceNm, landingWeightTon);

            return new CalculationResult()
            {
                TimeMin = timeMin,
                FuelTon = fuelTon,
                LandingWeightTon = landingWeightTon
            };
        }

        private double LandWeightTon()
        {
            double holdingFuelKg =
                para.HoldingMin * fuelData.HoldingFuelPerMinuteKg;

            return altnResult.LandingWeightTon +
                altnResult.FuelTon +
                (holdingFuelKg + para.MissedAppFuelKg + para.ExtraFuelKg)
                / 1000.0;
        }
    }
}
