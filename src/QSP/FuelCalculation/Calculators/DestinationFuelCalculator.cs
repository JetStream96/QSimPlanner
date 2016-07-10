namespace QSP.FuelCalculation.Calculators
{
    public class DestinationFuelCalculator
    {
        private FuelDataItem fuelData;
        private FuelParameters para;
        private AlternateFuelCalculator.Result altnResult;

        public DestinationFuelCalculator(
            FuelDataItem fuelData,
            FuelParameters para,
            AlternateFuelCalculator.Result altnResult)
        {
            this.fuelData = fuelData;
            this.para = para;
            this.altnResult = altnResult;
        }

        public Result Compute(double airDistanceNm)
        {
            double landingWeightTon = LandWeightTon();
            double timeMin =
                fuelData.FlightTimeTable.GetTimeMin(airDistanceNm);

            double fuelTon = fuelData.FuelTable.GetFuelRequired(
                airDistanceNm, landingWeightTon);

            return new Result()
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
                / 1000;
        }

        public class Result
        {
            public double TimeMin;
            public double FuelTon;
            public double LandingWeightTon;
        }
    }
}
