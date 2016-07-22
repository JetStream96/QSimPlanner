namespace QSP.WindAloft
{
    public sealed class DefaultWindTableCollection : IWindTableCollection
    {
        public WindUV GetWindUV(double lat, double lon, double altitudeFt)
        {
            return new WindUV(0.0, 0.0);
        }
    }
}
