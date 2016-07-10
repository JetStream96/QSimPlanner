using static QSP.MathTools.Doubles;

namespace QSP.FuelCalculation
{
    public class FuelCalculator
    {
        private FuelCalculationParameters para;
        private FuelDataItem data;

        private double landWeightTonAltn;
        private double landWeightTonDest;
        private double fuelToDestTon;
        private double fuelToAltnTon;
        private int timeToDestMin;
        private int timeToAltnMin;

        private bool computed;

        public FuelCalculator(
            FuelCalculationParameters para,
            FuelDataItem data)
        {
            this.para = para;
            this.data = data;
            computed = false;
        }

        public void ReCompute()
        {
            ComputePara();
        }

        public CalculateResult GetBriefResult()
        {
            ComputeIfNeeded();

            return new CalculateResult(
                landWeightTonAltn,
                landWeightTonDest,
                fuelToDestTon,
                fuelToAltnTon);
        }

        public FuelReportResult GetFullResult()
        {
            ComputeIfNeeded();

            return new FuelReportResult(
                fuelToDestTon,
                fuelToAltnTon,
                fuelToDestTon * 1000 * para.ContPercentKg / 100,
                para.ExtraFuelKg,
                para.HoldingMin * data.HoldingFuelPerMinuteKg,
                para.APUTime * data.ApuFuelPerMinKg,
                para.TaxiTime * data.TaxiFuelPerMinKg,
                para.FinalRsvMin * data.HoldingFuelPerMinuteKg,
                timeToDestMin,
                timeToAltnMin,
                RoundToInt(para.ExtraFuelKg / data.HoldingFuelPerMinuteKg),
                RoundToInt(para.HoldingMin),
                RoundToInt(para.FinalRsvMin),
                RoundToInt(para.APUTime),
                RoundToInt(para.TaxiTime));
        }

        private void ComputeIfNeeded()
        {
            if (computed == false)
            {
                ComputePara();
            }
        }

        private void ComputePara()
        {
            UpdateAltnLandWt();
            ComputeAltnPart();
            UpdateDestLandWt();
            ComputeDestPart();
        }
        
        private void ComputeAltnPart()
        {
            double airDistance = data.GtaTable.GetAirDistance(
                para.DisToAltn, para.AvgWindToAltn);

            timeToAltnMin =
                RoundToInt(data.FlightTimeTable.GetTimeMin(airDistance));

            fuelToAltnTon =
                data.FuelTable.GetFuelRequired(airDistance, landWeightTonAltn);
        }

        private void ComputeDestPart()
        {
            double airDistance = data.GtaTable.GetAirDistance(
                para.DisToDest, para.AvgWindToDest);

            timeToDestMin =
                RoundToInt(data.FlightTimeTable.GetTimeMin(airDistance));

            fuelToDestTon =
                data.FuelTable.GetFuelRequired(airDistance, landWeightTonDest);
        }

        private void UpdateDestLandWt()
        {
            double holdingFuelKg =
                para.HoldingMin * data.HoldingFuelPerMinuteKg;

            landWeightTonDest = landWeightTonAltn + fuelToAltnTon +
                (holdingFuelKg + para.MissedAppFuelKg + para.ExtraFuelKg)
                / 1000;
        }

        private void UpdateAltnLandWt()
        {
            double reserveFuelKg =
                para.FinalRsvMin * data.HoldingFuelPerMinuteKg;

            landWeightTonAltn = (para.ZfwKg + reserveFuelKg) / 1000;
        }

        public class CalculateResult
        {
            public double LandWeightTonAltn { get; private set; }
            public double LandWeightTonDest { get; private set; }
            public double FuelToDestTon { get; private set; }
            public double FuelToAltnTon { get; private set; }

            public CalculateResult(
                double LandWeightTonAltn,
                double LandWeightTonDest,
                double FuelToDestTon,
                double FuelToAltnTon)
            {
                this.LandWeightTonAltn = LandWeightTonAltn;
                this.LandWeightTonDest = LandWeightTonDest;
                this.FuelToDestTon = FuelToDestTon;
                this.FuelToAltnTon = FuelToAltnTon;
            }
        }
    }
}
