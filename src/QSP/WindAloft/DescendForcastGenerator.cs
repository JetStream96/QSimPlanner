using QSP.Common;

namespace QSP.WindAloft
{
    public class DescendForcastGenerator
    {
        private double lat;
        private double lon;
        private int[] FLs;

        public DescendForcastGenerator(double latitude, double longitude, int[] flightLevels)
        {
            lat = latitude;
            lon = longitude;
            FLs = flightLevels;
        }

        public Wind[] Generate()
        {
            var forcast = new Wind[FLs.Length];

            for (int i = 0; i < FLs.Length; i++)
            {
                var UVWind = QspCore.WxReader.GetWindUv(lat, lon, FLs[i]);
                forcast[i] = Wind.FromUV(UVWind);
            }

            return forcast;
        }
    }
}
