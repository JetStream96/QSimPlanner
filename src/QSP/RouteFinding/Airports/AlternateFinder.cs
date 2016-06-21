using QSP.MathTools;
using System.Collections.Generic;

namespace QSP.RouteFinding.Airports
{
    public class AlternateFinder
    {
        private AirportManager airportList;

        public AlternateFinder(AirportManager airportList)
        {
            this.airportList = airportList;
        }

        // length: if the max rwy length is smaller than this number then 
        // the result will not be shown to the user
        //
        // will return all suitable airports within certain distance until 
        // more than 10 results are found
        public List<Airport> GetListAltn(string dest, int length)
        {
            const int count = 10;
            const double disIncrement = 100;

            var result = new List<Airport>();
            var destLatLon = airportList.AirportLatlon(dest);

            double distance = 100.0;

            while (result.Count < count)
            {
                result = FilterResults(
                    airportList.Find(
                        destLatLon.Lat, destLatLon.Lon, distance),
                    dest,
                    length);

                distance += disIncrement;
            }
            return result;
        }

        private List<Airport> FilterResults(
            List<Airport> item, string dest, int length)
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

        // return a list of altn, with:
        // icao; name; max rwy length; dis from dest
        public List<AlternateInfo> AltnInfo(string dest, int length)
        {

            var altns = GetListAltn(dest, length);
            var result = new List<AlternateInfo>();

            foreach (var i in altns)
            {
                var latLon = airportList.AirportLatlon(dest);
                double distance = latLon.Distance(i.Lat, i.Lon);

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
