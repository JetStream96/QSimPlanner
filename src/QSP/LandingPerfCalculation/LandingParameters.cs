namespace QSP.LandingPerfCalculation
{
    public class LandingParameters
    {
        public double WeightKG { get; set; }
        public double RwyLengthMeter { get; set; }
        public double ElevationFT { get; set; }
        public double HeadwindKts { get; set; }
        public double SlopePercent { get; set; }
        public double TempCelsius { get; set; }
        public double QNH { get; set; }
        public double AppSpeedIncrease { get; set; }
        public int Reverser { get; set; }
        public int SurfaceCondition { get; set; }
        public int FlapsIndex { get; set; }
        public int BrakeIndex { get; set; }

        public LandingParameters() { }

        public LandingParameters(
            double WeightKG,
            double RwyLengthMeter,
            double ElevationFT,
            double HeadwindKts, 
            double SlopePercent,
            double TempCelsius,
            double QNH,
            double AppSpeedIncrease,
            int Reverser,
            int SurfaceCondition, 
            int FlapsIndex, 
            int BrakeIndex)
        {
            this.WeightKG = WeightKG;
            this.RwyLengthMeter = RwyLengthMeter;
            this.ElevationFT = ElevationFT;
            this.HeadwindKts = HeadwindKts;
            this.SlopePercent = SlopePercent;
            this.TempCelsius = TempCelsius;
            this.QNH = QNH;
            this.AppSpeedIncrease = AppSpeedIncrease;
            this.Reverser = Reverser;
            this.SurfaceCondition = SurfaceCondition;
            this.FlapsIndex = FlapsIndex;
            this.BrakeIndex = BrakeIndex;
        }
    }
}
