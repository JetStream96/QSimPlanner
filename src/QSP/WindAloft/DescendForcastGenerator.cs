namespace QSP.WindAloft
{
    public class DescendForcastGenerator
    {
        private IWindTableCollection windTables;
        private double lat;
        private double lon;
        private int[] FLs;

        public DescendForcastGenerator(
            IWindTableCollection windTables,
            double latitude,
            double longitude,
            int[] flightLevels)
        {
            this.windTables = windTables;
            lat = latitude;
            lon = longitude;
            FLs = flightLevels;
        }

        public Wind[] Generate()
        {
            var forcast = new Wind[FLs.Length];

            for (int i = 0; i < FLs.Length; i++)
            {
                var UVWind = windTables.GetWindUV(lat, lon, FLs[i] * 100.0);
                forcast[i] = Wind.FromUV(UVWind);
            }

            return forcast;
        }
    }
}
