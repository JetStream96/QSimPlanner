namespace QSP.TOPerfCalculation.Airbus.DataClasses
{
    public class Parameters : IHasOat
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

        /// <summary>
        /// 0: None, 1: Engine only, 2: All AI.
        /// </summary>
        public int AntiIce { get; set; }

        public bool PacksOn { get; set; }
        public string Flaps { get; set; }

        public Parameters() { }

        public Parameters(Parameters p)
        {
            RwyLengthMeter = p.RwyLengthMeter;
            RwyElevationFt = p.RwyElevationFt;
            RwyHeading = p.RwyHeading;
            RwySlopePercent = p.RwySlopePercent;
            WindHeading = p.WindHeading;
            WindSpeedKnots = p.WindSpeedKnots;
            OatCelsius = p.OatCelsius;
            QNH = p.QNH;
            SurfaceWet = p.SurfaceWet;
            WeightKg = p.WeightKg;
            AntiIce = p.AntiIce;
            PacksOn = p.PacksOn;
            Flaps = p.Flaps;
        }
    }
}
