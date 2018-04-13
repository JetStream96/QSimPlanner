namespace QSP.WindAloft
{
    public interface IWxTableCollection
    {
        WindUV GetWindUV(double lat, double lon, double altitudeFt);
        double GetTemp(double lat, double lon, double altitudeFt);
    }
}
