namespace QSP.WindAloft
{
    public interface IWindTableCollection
    {
        WindUV GetWindUV(double lat, double lon, double altitudeFt);
    }
}
