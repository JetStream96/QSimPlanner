using System;
using System.Collections.Generic;
using static QSP.RouteFinding.RouteFindingCore;

namespace QSP.RouteFinding.Airports
{

    public static class AlternateFinder
    {

        public static List<Airport> GetListAltn(string dest, int length)
        {
            //length: if the max rwy length is smaller than this number then the result will not be shown to the user
            //will return all suitable airports within certain distance until more than 10 results are found

            const int COUNT = 10;
            const double DIS_INCR = 100;

            var result = new List<Airport>();
            var destLatLon = AirportList.AirportLatlon(dest);

            double distance = 100.0;

            while (result.Count < COUNT)
            {
                result = filterResults(AirportList.Find(destLatLon.Lat, destLatLon.Lon, distance), dest, length);
                distance += DIS_INCR;
            }
            return result;
        }

        private static List<Airport> filterResults(List<Airport> item, string dest, int length)
        {
            var result = new List<Airport>();

            foreach (var i in item)
            {
                if (i.LongestRwyLength >= length && i.Icao != dest)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public static List<AlternateInfo> AltnInfo(string dest, int length)
        {
            //return a list of altn, with:
            //icao; name; max rwy length; dis from dest

            var altns = GetListAltn(dest, length);
            var result = new List<AlternateInfo>();

            foreach (var i in altns)
            {
                result.Add(new AlternateInfo(i.Icao, i.Name, i.LongestRwyLength,
                    (int)AirportList.AirportLatlon(dest).Distance(i.Lat, i.Lon)));
            }
            result.Sort(AlternateInfo.AltnDisComparer());
            return result;
        }

        #region Container Class

        public class AlternateInfo
        {
            public string Icao { get; set; }
            public string AirportName { get; set; }
            public int LongestRwyLength { get; set; }
            public int Distance { get; set; }

            public AlternateInfo(string Icao, string AirportName, int LongestRwyLength, int Distance)
            {
                this.Icao = Icao;
                this.AirportName = AirportName;
                this.LongestRwyLength = LongestRwyLength;
                this.Distance = Distance;
            }

            public static IComparer<AlternateInfo> AltnDisComparer()
            {
                return new altnDisSortHelper();
            }

            private class altnDisSortHelper : IComparer<AlternateInfo>
            {
                public int Compare(AlternateInfo x, AlternateInfo y)
                {
                    return x.Distance.CompareTo(y.Distance);
                }
            }
        }

        #endregion

    }
}
