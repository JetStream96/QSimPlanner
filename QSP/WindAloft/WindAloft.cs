using System;
using System.Net;
using QSP.AviationTools;
using QSP.RouteFinding.Containers;
using QSP.Core;

namespace QSP.WindAloft
{

     public  static class Utilities
    {

        public static int[] FullWindDataSet = { 100, 200, 250, 300, 350, 400, 500, 600, 700, 850 };

        public static string wxFileDirectory = QspCore.QspLocalDirectory + "\\Wx\\tmp";
        public static string CheckCurrentUrl()
        {
            using (var client = new WebClient())
            {
                //download the entire source code of the webpage
                return client.DownloadString("http://nomads.ncep.noaa.gov/cgi-bin/filter_gfs_1p00.pl");
                //old url: http://nomads.ncep.noaa.gov/cgi-bin/filter_gfs.pl
            }
        }

        public static int[] GetTwoDatasets(double FL)
        {
            double press = CoversionTools.AltToPressureMb(FL * 100);

            //if the altitude is too low, such that pressure is higher than the highest available, then use the largest pressure value
            //e.g. pressure > 850mb (about 5000 ft), use 850 mb wind
            if (press >= FullWindDataSet[FullWindDataSet.Length - 1])
            {
                return new int[]{
                    FullWindDataSet[FullWindDataSet.Length - 2],
                    FullWindDataSet[FullWindDataSet.Length - 1]
                };
            }


            for (int i = 0; i <= FullWindDataSet.Length - 2; i++)
            {
                if (press >= FullWindDataSet[i] & press <= FullWindDataSet[i + 1])
                {
                    return new int[]{
                        FullWindDataSet[i],
                        FullWindDataSet[i + 1]
                    };
                }

            }

            throw new Exception("Cruising altitude out of range.");
        }

        public static int AvgTailWind(Route rte, int cruizeLevel, int tas)
        {

            //returns the avg tailwind to dest and altn, respectifully
            //based on the route generated/built from RouteGen page
            //assuming all wx are downloaded and decoded from grib2
            //flight levels have to be enered on main page

            double AirDisToDest = 0;
            double GrdDisToDest = 0;

            for (int i = 0; i <= rte.Waypoints.Count - 2; i++)
            {
                Tuple<double, double> t = GetAirDisGrdDis(rte.Waypoints[i].LatLon, rte.Waypoints[i + 1].LatLon, tas, cruizeLevel);
                AirDisToDest += t.Item1;
                GrdDisToDest += t.Item2;
            }

            return Convert.ToInt32(Math.Round(tas * (GrdDisToDest / AirDisToDest - 1)));

        }

        public static Tuple<double, double> GetAirDisGrdDis(LatLon  latlon1, LatLon latlon2, int tas, double FL)
        {
            //returns airdis and grdDis

            double dis = MathTools.MathTools.GreatCircleDistance(latlon1, latlon2);

            AvgWindCalculator AvgWindCalc = new AvgWindCalculator(QspCore.WxReader, tas, FL);
            AvgWindCalc.SetPoint1(latlon1);
            AvgWindCalc.SetPoint2(latlon2);

            double avgWind = AvgWindCalc.GetAvgWind(1.0);

            return new Tuple<double, double>(dis * tas / (tas + avgWind), dis);

        }

    }
}
