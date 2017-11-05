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
        // The number of returned airports is no less than minCount, unless
        // there isn't enough in the airportList.
        public IList<IAirport> GetListAltn(string dest, int lengthFt, int minCount = 10)
        {
            double distance = 100.0;
            const double disMultiplyFactor = 2.0;
            var destAirport = airportList[dest];

            Func<IAirport, bool> selector = i =>
                i.LongestRwyLengthFt >= lengthFt && i.Icao != dest;

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

        /// <summary>
        /// Returns a list of alternates. dest must be in airportList.
        /// </summary>
        public IList<AlternateInfo> AltnInfo(string dest, int length)
        {
            var altns = GetListAltn(dest, length);
            var result = altns.Select(i =>
            {
                double distance = airportList[dest].Distance(i);

                return new AlternateInfo(i.Icao, i.Name, i.LongestRwyLengthFt,
                    Numbers.RoundToInt(distance));
            }).ToList();

            return result;
        }

        public class AlternateInfo
        {
            public string Icao { get; private set; }
            public string AirportName { get; private set; }
            public int LongestRwyLengthFt { get; private set; }
            public int Distance { get; private set; }

            public AlternateInfo(
                string Icao,
                string AirportName,
                int LongestRwyLengthFt,
                int Distance)
            {
                this.Icao = Icao;
                this.AirportName = AirportName;
                this.LongestRwyLengthFt = LongestRwyLengthFt;
                this.Distance = Distance;
            }
        }
    }
}
