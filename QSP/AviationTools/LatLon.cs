namespace QSP.AviationTools
{

    public class LatLon
    {

        public double Lat { get; set; }
        public double Lon { get; set; }

        public LatLon()
        {
        }

        public LatLon(double lat, double lon)
        {
            this.Lat = lat;
            this.Lon = lon;
        }

        /// <summary>
        /// The great circle distance between two points, in nautical miles.
        /// </summary>
        public double Distance(double lat, double lon)
        {
            return MathTools.MathTools.GreatCircleDistance(this.Lat, this.Lon, lat, lon);
        }

        /// <summary>
        /// The great circle distance between two points, in nautical miles.
        /// </summary>
        public double Distance(LatLon LatLon)
        {
            return MathTools.MathTools.GreatCircleDistance(this.Lat, this.Lon, LatLon.Lat, LatLon.Lon);
        }

    }

}

