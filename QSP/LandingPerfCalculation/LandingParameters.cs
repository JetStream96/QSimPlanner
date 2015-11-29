namespace QSP.LandingPerfCalculation
{

    public class LandingParameters
    {

        public int WeightKG { get; set; }
        public int RwyLengthMeter { get; set; }
        public int ElevationFT { get; set; }
        public int HeadwindKts { get; set; }
        public double SlopePercent { get; set; }
        public int TempCelsius { get; set; }
        public int AppSpeedIncrease { get; set; }
        public ReverserOption Reverser { get; set; }
        public SurfaceCondition SurfaceCondition { get; set; }
        public int FlapsIndex { get; set; }
        public int AutoBrakeIndex { get; set; }

        public LandingParameters(int WeightKG, int RwyLengthMeter, int ElevationFT, int HeadwindKts, double SlopePercent,
                                 int TempCelsius, int AppSpeedIncrease, ReverserOption Reverser,
                                 SurfaceCondition SurfaceCondition, int FlapsIndex, int AutoBrakeIndex)
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
