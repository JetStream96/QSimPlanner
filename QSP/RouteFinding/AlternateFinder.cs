using System;
using System.Collections.Generic;

namespace QSP.RouteFinding
{

    public static class AlternateFinder
    {

        public static List<AirportData> GetListAltn(string dest, int length)
        {
            //length: if the max rwy length is smaller than this number then the result will not be shown to the user
            //will return all suitable airports within certain distance until more than 10 results are found

            const int COUNT = 10;
            const double DIS_INCR = 100;

            List<AirportData> result = new List<AirportData>();
            var  destLatLon = RouteFindingCore.AirportList.AirportLatlon(dest);

            double distance = 100.0;

            while (result.Count < COUNT)
            {
                result = filterResults(RouteFindingCore.AirportFinder.Find(destLatLon.Lat, destLatLon.Lon, distance), dest, length);
                distance += DIS_INCR;
            }

            return result;

        }

        private static List<AirportData> filterResults(List<AirportData> item, string dest, int length)
        {

            List<AirportData> result = new List<AirportData>();

            foreach (var i in item)
            {
                if (i.LongestRwyLength >= length && i.Icao != dest)
                {
                    result.Add(i);
                }
            }

            return result;

        }

        public static List<Tuple<string, string, int, int>> AltnInfo(string dest, int length)
        {
            //return a list of altn, with:
            //icao; name; max rwy length; dis from dest

            var altns = AlternateFinder.GetListAltn(dest, length);

            List<Tuple<string, string, int, int>> result = new List<Tuple<string, string, int, int>>();

            foreach (var i in altns)
            {
                result.Add(new Tuple<string, string, int, int>(i.Icao, i.Name, i.LongestRwyLength, 
                    Convert.ToInt32(RouteFindingCore.AirportList.AirportLatlon(dest).Distance(i.Lat, i.Lon))));
            }

            result.Sort(altnInfoComparer());

            return result;

        }

        private static IComparer<Tuple<string, string, int, int>> altnInfoComparer()
        {
            return new altnInfoSortHelper();
        }

        private class altnInfoSortHelper : IComparer<Tuple<string, string, int, int>>
        {

            public int Compare(Tuple<string, string, int, int> x, Tuple<string, string, int, int> y)
            {

                return x.Item4.CompareTo(y.Item4);

            }
        }


    }

}
