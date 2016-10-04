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
            var landingResult = GetLandingResult();
            double timeMin =
                fuelData.FlightTimeTable.GetTimeMin(airDistanceNm);

            double fuelTon = fuelData.FuelTable.GetFuelRequiredTon(
                airDistanceNm, landingResult.LandWeightTon);

            return new CalculationResult()
            {
                TimeMin = timeMin,
                FuelTon = fuelTon,
                LandingWeightTon = landingResult.LandWeightTon,
                HoldingFuelKg = landingResult.HoldingFuelKg
            };
        }

        private struct LandingResult
        {
            public double LandWeightTon, HoldingFuelKg;
        }

        private LandingResult GetLandingResult()
        {
            var wtWithoutHoldingTon = altnResult.LandingWeightTon +
                altnResult.FuelTon +
                (para.MissedAppFuelKg + para.ExtraFuelKg) / 1000.0;

            var holdingFuelKg = fuelData.HoldingFuelKg(para.HoldingMin,
                wtWithoutHoldingTon);

            return new LandingResult
            {
                LandWeightTon = wtWithoutHoldingTon + holdingFuelKg / 1000.0,
                HoldingFuelKg = holdingFuelKg
            };
        }
    }
}
