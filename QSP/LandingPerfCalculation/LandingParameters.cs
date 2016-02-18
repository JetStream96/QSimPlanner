namespace QSP.LandingPerfCalculation
{
    public class LandingParameters
    {
        public int WeightKG { get; private set; }
        public int RwyLengthMeter { get; private set; }
        public int ElevationFT { get; private set; }
        public int HeadwindKts { get; private set; }
        public double SlopePercent { get; private set; }
        public int TempCelsius { get; private set; }
        public int AppSpeedIncrease { get; private set; }
        public ReverserOption Reverser { get; private set; }
        public SurfaceCondition SurfaceCondition { get; private set; }
        public int FlapsIndex { get; private set; }
        public int AutoBrakeIndex { get; private set; }

        public LandingParameters(
            int WeightKG, 
            int RwyLengthMeter, 
            int ElevationFT, 
            int HeadwindKts, 
            double SlopePercent,
            int TempCelsius, 
            int AppSpeedIncrease, 
            ReverserOption Reverser,
            SurfaceCondition SurfaceCondition, 
            int FlapsIndex, 
            int AutoBrakeIndex)
        {
            this.WeightKG = WeightKG;
            this.RwyLengthMeter = RwyLengthMeter;
            this.ElevationFT = ElevationFT;
            this.HeadwindKts = HeadwindKts;
            this.SlopePercent = SlopePercent;
            this.TempCelsius = TempCelsius;
            this.AppSpeedIncrease = AppSpeedIncrease;
            this.Reverser = Reverser;
            this.SurfaceCondition = SurfaceCondition;
            this.FlapsIndex = FlapsIndex;
            this.AutoBrakeIndex = AutoBrakeIndex;

        }
    }
}
