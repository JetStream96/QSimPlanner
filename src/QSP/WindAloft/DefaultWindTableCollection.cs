namespace QSP.WindAloft
{
    public sealed class DefaultWindTableCollection : IWxTableCollection
    {
        public double GetTemp(double lat, double lon, double altitudeFt) => 0;

        public WindUV GetWindUV(double lat, double lon, double altitudeFt)
        {
            return new WindUV(0.0, 0.0);
        }
    }
}
