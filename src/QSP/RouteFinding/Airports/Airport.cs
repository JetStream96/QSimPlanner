using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.Airports
{
    public class Airport : ICoordinate, IEquatable<Airport>
    {
        private List<RwyData> _rwys;

        public string Icao { get; private set; }
        public string Name { get; private set; }
        public double Lat { get; private set; }
        public double Lon { get; private set; }
        public int Elevation { get; private set; }
        public bool TransAvail { get; private set; }
        public int TransAlt { get; private set; }
        public int TransLvl { get; private set; }
        public int LongestRwyLength { get; private set; }

        public IReadOnlyList<RwyData> Rwys { get { return _rwys; } }

        public Airport(
            string Icao,
            string Name,
            double Lat,
            double Lon,
            int Elevation,
            bool TransAvail,
            int TransAlt,
            int TransLvl,
            int LongestRwyLength,
            List<RwyData> Rwys)
        {
            this.Icao = Icao;
            this.Name = Name;
            this.Lat = Lat;
            this.Lon = Lon;
            this.Elevation = Elevation;
            this.TransAvail = TransAvail;
            this.TransAlt = TransAlt;
            this.TransLvl = TransLvl;
            this.LongestRwyLength = LongestRwyLength;
            _rwys = Rwys;
        }

        public Airport(Airport item)
        {
            Icao = item.Icao;
            Name = item.Name;
            Lat = item.Lat;
            Lon = item.Lon;
            Elevation = item.Elevation;
            TransAvail = item.TransAvail;
            TransAlt = item.TransAlt;
            TransLvl = item.TransLvl;
            LongestRwyLength = item.LongestRwyLength;
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
        
        public bool Equals(Airport other)
        {
            return other != null && Icao == other.Icao;
        }

        public override int GetHashCode()
        {
            return Icao.GetHashCode();
        }
    }
}
