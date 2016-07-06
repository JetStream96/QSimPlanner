using static QSP.MathTools.Doubles;

namespace QSP.FuelCalculation
{
    public class FuelCalculatorNew
    {
        private double landWeightTonAltn;
        private double landWeightTonDest;
        private double fuelToDestTon;
        private double fuelToAltnTon;
        private int timeToDestMin;
        private int timeToAltnMin;

        private FuelCalculationParameters para;
        private FuelData data;

        public FuelCalculatorNew(
            FuelCalculationParameters para,
            FuelData data)
        {
            this.para = para;
            this.data = data;
        }

        public FuelReportResult Calculate()
        {
            ComputePara();

            return new FuelReportResult(
                fuelToDestTon,
                fuelToAltnTon,
                fuelToDestTon * 1000 * para.ContPerc / 100,
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
    }
}
