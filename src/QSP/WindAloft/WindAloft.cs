using System;
using QSP.Common;
using QSP.RouteFinding.Routes;
using QSP.AviationTools.Coordinates;

namespace QSP.WindAloft
{

    public static class Utilities
    {
        public static int[] FullWindDataSet = { 100, 200, 250, 300, 350, 400, 500, 600, 700, 850 };
        public static string WxFileDirectory = "Wx\\tmp";

        public static int AvgTailWind(Route rte, int cruizeLevel, int tas)
        {
            //returns the avg tailwind to dest and altn, respectifully
            //based on the route generated/built from RouteGen page
            //assuming all wx are downloaded and decoded from grib2
            //flight levels have to be enered on main page

            double AirDisToDest = 0.0;
            double GrdDisToDest = 0.0;

            var node = rte.First;

            while (node != rte.Last)
            {
                var t = GetAirDisGrdDis(node.Value.Waypoint.LatLon,
                    node.Next.Value.Waypoint.LatLon, 
                    tas, 
                    cruizeLevel);

                AirDisToDest += t.Item1;
                GrdDisToDest += t.Item2;
                node = node.Next;
            }

            return (int)(tas * (GrdDisToDest / AirDisToDest - 1.0));
        }

        public static Tuple<double, double> GetAirDisGrdDis(LatLon latlon1, LatLon latlon2, int tas, double FL)
        {
            //returns airdis and grdDis

            double dis = latlon1.Distance(latlon2);

            var AvgWindCalc = new AvgWindCalculator(QspCore.WxReader, tas, FL);
            AvgWindCalc.SetPoint1(latlon1);
            AvgWindCalc.SetPoint2(latlon2);

            double avgWind = AvgWindCalc.GetAvgWind(1.0);

            return new Tuple<double, double>(dis * tas / (tas + avgWind), dis);

        }

    }
}
