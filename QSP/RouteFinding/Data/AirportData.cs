using QSP.AviationTools;
using System.Collections.Generic;
namespace QSP.RouteFinding
{

    public class AirportData
    {

        public string Icao { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int Elevation { get; set; }
        public int TransAlt { get; set; }
        public int TransLvl { get; set; }
        public int LongestRwyLength { get; set; }

        public List<RwyData> Rwys;
        public AirportData(string icaoCode)
        {
            Icao = icaoCode;
        }

        public AirportData(string icaoCode, string airportName, double latitude, double longitude, int elev, int transAltitude, int transLevel)
        {
            Icao = icaoCode;
            Name = airportName;
            Lat = latitude;
            Lon = longitude;
            Elevation = elev;
            TransAlt = transAltitude;
            TransLvl = transLevel;
            Rwys = new List<RwyData>();
        }

        public AirportData(AirportData item)
        {
            Icao = item.Icao;
            Name = item.Name;
            Lat = item.Lat;
            Lon = item.Lon;
            Elevation = item.Elevation;
            TransAlt = item.TransAlt;
            TransLvl = item.TransLvl;
            Rwys = item.Rwys.ConvertAll(rwy => new RwyData(rwy));
        }

        public static IComparer<AirportData> CompareIcao()
        {
            return new compIcaoHelper();
        }

        private class compIcaoHelper : IComparer<AirportData>
        {

            public int Compare(AirportData x, AirportData y)
            {
                return x.Icao.CompareTo(y.Icao);
            }
        }

        public static LatLon LatLon(AirportData item)
        {
            return new LatLon(item.Lat, item.Lon);
        }

    }

}
