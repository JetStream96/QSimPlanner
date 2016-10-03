using QSP.MathTools;
using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Constants;
using static System.Math;

namespace QSP.RouteFinding.Airports
{
    public class AlternateFinder
    {
        private AirportManager airportList;

        public AlternateFinder(AirportManager airportList)
        {
            this.airportList = airportList;
        }

        // lengthFt: minimum runway length.
        //
        // The number of returned airports is larger than minCount, unless
        // there isn't enough in the airportList.
        public List<Airport> GetListAltn(
            string dest, int lengthFt, int minCount = 10)
        {
            double distance = 100.0;
            const double disMultiplyFactor = 2.0;
            var destAirport = airportList[dest];

            Func<Airport, bool> selector = i =>
            i.LongestRwyLength >= lengthFt && i.Icao != dest;

            while (true)
            {
                var results = airportList
                    .Find(destAirport.Lat, destAirport.Lon, distance)
                    .Where(selector)
                    .ToList();

                if (results.Count >= minCount || distance > EarthRadiusNm * PI)
                {
                    return results;
                }

                distance *= disMultiplyFactor;
            }
        }

        // return a list of altn, with:
        // icao; name; max rwy length; dis from dest
        public List<AlternateInfo> AltnInfo(string dest, int length)
        {

            var altns = GetListAltn(dest, length);
            var result = new List<AlternateInfo>();

            foreach (var i in altns)
            {
                double distance = airportList[dest].Distance(i);

                result.Add(
                    new AlternateInfo(
                        i.Icao, i.Name, i.LongestRwyLength,
                        Doubles.RoundToInt(distance)));
            }

            result.Sort(AlternateInfo.AltnDisComparer());
            return result;
        }

        #region Container Class

        public class AlternateInfo
        {
            public string Icao { get; private set; }
            public string AirportName { get; private set; }
            public int LongestRwyLength { get; private set; }
            public int Distance { get; private set; }

            public AlternateInfo(
                string Icao,
                string AirportName,
                int LongestRwyLength,
                int Distance)
            {
                this.Icao = Icao;
                this.AirportName = AirportName;
                this.LongestRwyLength = LongestRwyLength;
                this.Distance = Distance;
            }

            public static Comparer<AlternateInfo> AltnDisComparer()
            {
                return new altnDisSortHelper();
            }

            private class altnDisSortHelper : Comparer<AlternateInfo>
            {
                public override int Compare(AlternateInfo x, AlternateInfo y)
                {
                    return x.Distance.CompareTo(y.Distance);
                }
            }
        }

        #endregion

    }
}
