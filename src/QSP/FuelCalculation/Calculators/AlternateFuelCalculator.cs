namespace QSP.FuelCalculation.Calculators
{
    public class AlternateFuelCalculator
    {
        private FuelDataItem fuelData;
        private FuelParameters para;

        public AlternateFuelCalculator(
            FuelDataItem fuelData, FuelParameters para)
        {
            this.fuelData = fuelData;
            this.para = para;
        }

        public CalculationResult Compute(double airDistanceNm)
        {
            double landingWeightTon = LandWeightTon();
            double timeMin =
                fuelData.FlightTimeTable.GetTimeMin(airDistanceNm);

            double fuelTon = fuelData.FuelTable.GetFuelRequired(
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
            double reserveFuelKg =
                para.FinalRsvMin * fuelData.HoldingFuelPerMinuteKg;

            return (para.ZfwKg + reserveFuelKg) / 1000.0;
        }        
    }
}
