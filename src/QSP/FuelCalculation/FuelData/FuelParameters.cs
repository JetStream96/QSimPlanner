namespace QSP.FuelCalculation.FuelData
{
    // Unit of fuel is in kg, time in minutes.
    public class FuelParameters
    {
        public double Zfw { get; }
        public double ContPercent { get; }
        public double MissedAppFuel { get; }
        public double HoldingTime { get; }
        public double ExtraFuel { get; }
        public double ApuTime { get; }
        public double TaxiTime { get; }
        public double FinalRsvTime { get; }
        public FuelDataItem FuelData { get; }

        public FuelParameters(
             double Zfw,
             double ContPercent,
             double MissedAppFuel,
             double HoldingTime,
             double ExtraFuel,
             double ApuTime,
             double TaxiTime,
             double FinalRsvTime,
             double FuelBias,
             FuelDataItem FuelData)
        {
            this.Zfw = Zfw;
            this.ContPercent = ContPercent;
            this.MissedAppFuel = MissedAppFuel;
            this.HoldingTime = HoldingTime;
            this.ExtraFuel = ExtraFuel;
            this.ApuTime = ApuTime;
            this.TaxiTime = TaxiTime;
            this.FinalRsvTime = FinalRsvTime;
            this.FuelData = FuelData.WithBias(FuelBias);
        }
    }
}

