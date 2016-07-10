using QSP.Utilities.Units;

namespace QSP.FuelCalculation
{
    public class FuelParameters
    {
        public double ZfwKg { get; private set; }
        public double ContPercent { get; private set; }
        public double MissedAppFuelKg { get; private set; }
        public double HoldingMin { get; private set; }
        public double ExtraFuelKg { get; private set; }
        public double ApuTimeMin { get; private set; }
        public double TaxiTimeMin { get; private set; }
        public double FinalRsvMin { get; private set; }
        public double DisToDestNm { get; private set; }
        public double DisToAltnNm { get; private set; }
        public double AvgWindToDest { get; private set; }
        public double AvgWindToAltn { get; private set; }
        public WeightUnit WtUnit { get; private set; }
        public FuelDataItem FuelData { get; private set; }
        
        public FuelParameters(
             double ZfwKg,
             double ContPercent,
             double MissedAppFuelKg,
             double HoldingMin,
             double ExtraFuelKg,
             double ApuTimeMin,
             double TaxiTimeMin,
             double FinalRsvMin,
             double DisToDestNm,
             double DisToAltnNm,
             double AvgWindToDest,
             double AvgWindToAltn,
             WeightUnit WtUnit,
             FuelDataItem FuelData)
        {
            this.ZfwKg = ZfwKg;
            this.ContPercent = ContPercent;
            this.MissedAppFuelKg = MissedAppFuelKg;
            this.HoldingMin = HoldingMin;
            this.ExtraFuelKg = ExtraFuelKg;
            this.ApuTimeMin = ApuTimeMin;
            this.TaxiTimeMin = TaxiTimeMin;
            this.FinalRsvMin = FinalRsvMin;
            this.DisToDestNm = DisToDestNm;
            this.DisToAltnNm = DisToAltnNm;
            this.AvgWindToDest = AvgWindToDest;
            this.AvgWindToAltn = AvgWindToAltn;
            this.WtUnit = WtUnit;
            this.FuelData = FuelData;
        }
    }
}

