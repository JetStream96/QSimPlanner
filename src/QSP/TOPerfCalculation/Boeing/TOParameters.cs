using QSP.TOPerfCalculation.Boeing.PerfData;

namespace QSP.TOPerfCalculation.Boeing
{
    public class TOParameters : IHasOat
    {
        public double RwyLengthMeter { get; private set; }
        public double RwyElevationFt { get; private set; }
        public double RwyHeading { get; private set; }
        public double RwySlopePercent { get; private set; }
        public double WindHeading { get; private set; }
        public double WindSpeedKnots { get; private set; }
        public double OatCelsius { get; private set; }
        public double QNH { get; private set; }
        public bool SurfaceWet { get; private set; }
        public double WeightKg { get; private set; }

        // From 0 to IndividualPerfTable.AlternateThrustTables.Count
        public int ThrustRating { get; private set; }

        public AntiIceOption AntiIce { get; private set; }
        public bool PacksOn { get; private set; }

        // From 0 to BoeingPerfTable.Flaps.Count - 1.
        public int FlapsIndex { get; private set; }

        public TOParameters(
            double RwyLengthMeter,
            double RwyElevationFt,
            double RwyHeading,
            double RwySlopePercent,
            double WindHeading,
            double WindSpeed,
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
            this.WindSpeedKnots = WindSpeed;
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
