namespace QSP.FuelCalculation.FuelDataNew
{
    // Unit of fuel is in kg, time in minutes.
    public class FuelParameters
    {
        public double Zfw { get; private set; }
        public double ContPercent { get; private set; }
        public double MissedAppFuel { get; private set; }
        public double HoldingTime { get; private set; }
        public double ExtraFuel { get; private set; }
        public double ApuTime { get; private set; }
        public double TaxiTime { get; private set; }
        public double FinalRsvTime { get; private set; }
        public double FuelBias { get; private set; }
        public FuelDataItem FuelData { get; private set; }

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
            this.FuelBias = FuelBias;
            this.FuelData = FuelData;
        }
    }
}

