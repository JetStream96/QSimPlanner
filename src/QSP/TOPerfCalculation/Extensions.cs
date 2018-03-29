namespace QSP.TOPerfCalculation
{
    public static class Extensions
    {
        public static TOParameters CloneWithOat(this TOParameters p , double oatCelcius)
        {
            return new TOParameters(
                p.RwyLengthMeter, 
                p.RwyElevationFt,
                p.RwyHeading,
                p.RwySlopePercent,
                p.WindHeading,
                p.WindSpeedKnots,
                oatCelcius,
                p.QNH,
                p.SurfaceWet,
                p.WeightKg,
                p.ThrustRating,
                p.AntiIce,
                p.PacksOn, 
                p.FlapsIndex);
        }
    }
}
