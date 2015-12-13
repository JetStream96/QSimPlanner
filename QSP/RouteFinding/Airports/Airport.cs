using QSP.AviationTools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using QSP.RouteFinding.Data;

namespace QSP.RouteFinding.Airports
{
    public class Airport : ICoordinate
    {
        #region Fields

        private string _icao;
        private string _name;
        private double _lat;
        private double _lon;
        private int _elevation;
        private int _transAlt;
        private int _transLvl;
        private int _longestRwyLength;
        private List<RwyData> _rwys;

        #endregion

        #region Properties

        public string Icao { get { return _icao; } }
        public string Name { get { return _name; } }
        public double Lat { get { return _lat; } }
        public double Lon { get { return _lon; } }
        public int Elevation { get { return _elevation; } }
        public int TransAlt { get { return _transAlt; } }
        public int TransLvl { get { return _transLvl; } }
        public int LongestRwyLength { get { return _longestRwyLength; } }

        public ReadOnlyCollection<RwyData> Rwys
        {
            get
            {
                return _rwys.AsReadOnly();
            }
        }

        #endregion

        public Airport(string Icao, string Name, double Lat, double Lon, int Elevation, int TransAlt,
                       int TransLvl, int LongestRwyLength, List<RwyData> Rwys)
        {
            _icao = Icao;
            _name = Name;
            _lat = Lat;
            _lon = Lon;
            _elevation = Elevation;
            _transAlt = TransAlt;
            _transLvl = TransLvl;
            _longestRwyLength = LongestRwyLength;
            _rwys = Rwys;
        }

        public Airport(Airport item)
        {
            _icao = item.Icao;
            _name = item.Name;
            _lat = item.Lat;
            _lon = item.Lon;
            _elevation = item.Elevation;
            _transAlt = item.TransAlt;
            _transLvl = item.TransLvl;
            _longestRwyLength = item.LongestRwyLength;
            _rwys = new List<RwyData>(item.Rwys);
        }

        public static IComparer<Airport> CompareIcao()
        {
            return new compIcaoHelper();
        }

        private class compIcaoHelper : IComparer<Airport>
        {
            public int Compare(Airport x, Airport y)
            {
                return x.Icao.CompareTo(y.Icao);
            }
        }

        public static LatLon LatLon(Airport item)
        {
            return new LatLon(item.Lat, item.Lon);
        }

    }

}
