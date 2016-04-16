using QSP.LandingPerfCalculation.Boeing.PerfData;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class LandingParameters
    {
        public double WeightKG { get; private set; }
        public double RwyLengthMeter { get; private set; }
        public double ElevationFT { get; private set; }
        public double HeadwindKts { get; private set; }
        public double SlopePercent { get; private set; }
        public double TempCelsius { get; private set; }
        public double QNH { get; private set; }
        public double AppSpeedIncrease { get; private set; }
        public ReverserOption Reverser { get; private set; }
        public SurfaceCondition SurfaceCondition { get; private set; }
        public int FlapsIndex { get; private set; }
        public int BrakeIndex { get; private set; }

        public LandingParameters(
            double WeightKG,
            double RwyLengthMeter,
            double ElevationFT,
            double HeadwindKts, 
            double SlopePercent,
            double TempCelsius,
            double QNH,
            double AppSpeedIncrease, 
            ReverserOption Reverser,
            SurfaceCondition SurfaceCondition, 
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
