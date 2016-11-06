namespace QSP.FuelCalculation.Calculators
{/*
    // Bias is applied in this calculator.
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
            var landingResult = GetLandingResult();
            double timeMin =
                fuelData.FlightTimeTable.GetTimeMin(airDistanceNm);

            double fuelTon = fuelData.FuelTable.GetFuelRequiredTon(
                airDistanceNm, landingResult.LandWeightTon) * para.FuelBias;

            return new CalculationResult()
            {
                TimeMin = timeMin,
                FuelTon = fuelTon,
                LandingWeightTon = landingResult.LandWeightTon,
                HoldingFuelKg = landingResult.ReserveFuelKg
            };
        }

        private struct LandingResult
        {
            public double LandWeightTon, ReserveFuelKg;
        }

        private LandingResult GetLandingResult()
        {
            double reserveFuelKg = fuelData.HoldingFuelKg(para.FinalRsvMin,
                para.ZfwKg / 1000.0) * para.FuelBias;

            return new LandingResult
            {
                LandWeightTon = (para.ZfwKg + reserveFuelKg) / 1000.0,
                ReserveFuelKg = reserveFuelKg
            };
        }
    }*/
}
