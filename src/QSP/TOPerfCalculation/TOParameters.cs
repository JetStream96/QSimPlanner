namespace QSP.TOPerfCalculation
{
    public class TOParameters : IHasOat
    {
        public double RwyLengthMeter { get; set; }
        public double RwyElevationFt { get; set; }
        public double RwyHeading { get; set; }
        public double RwySlopePercent { get; set; }
        public double WindHeading { get; set; }
        public double WindSpeedKnots { get; set; }
        public double OatCelsius { get; set; }
        public double QNH { get; set; }
        public bool SurfaceWet { get; set; }
        public double WeightKg { get; set; }

        // Starts from 0 to total number of thrust ratings.
        public int ThrustRating { get; set; }

        public AntiIceOption AntiIce { get; set; }
        public bool PacksOn { get; set; }

        // From 0 to total number of flaps options.
        public int FlapsIndex { get; set; }

        public TOParameters() { }

        public TOParameters(
            double RwyLengthMeter,
            double RwyElevationFt,
            double RwyHeading,
            double RwySlopePercent,
            double WindHeading,
            double WindSpeedKnots,
            double OatCelsius,
            double QNH,
            bool SurfaceWet,
            double WeightKg,
            int ThrustRating,
            AntiIceOption AntiIce,
            bool PacksOn,
            int FlapsIndex)
        {
            this.RwyLengthMeter = RwyLengthMeter;
            this.RwyElevationFt = RwyElevationFt;
            this.RwyHeading = RwyHeading;
            this.RwySlopePercent = RwySlopePercent;
            this.WindHeading = WindHeading;
            this.WindSpeedKnots = WindSpeedKnots;
            this.OatCelsius = OatCelsius;
            this.QNH = QNH;
            this.SurfaceWet = SurfaceWet;
            this.WeightKg = WeightKg;
            this.ThrustRating = ThrustRating;
            this.AntiIce = AntiIce;
            this.PacksOn = PacksOn;
            this.FlapsIndex = FlapsIndex;
        }
    }
}
