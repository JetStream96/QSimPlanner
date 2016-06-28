using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System.Collections.Generic;
using static QSP.MathTools.Doubles;

namespace QSP.WindAloft
{
    public static class Utilities
    {
        private static int[] _fullWindDataSet =
            { 100, 200, 250, 300, 350, 400, 500, 600, 700, 850 };

        public static IReadOnlyList<int> FullWindDataSet
        {
            get
            {
                return _fullWindDataSet;
            }
        }

        public static readonly string WxFileDirectory = @"Wx\tmp";

        /// <summary>
        /// The average tailwind of the route.
        /// </summary>
        public static int AvgTailWind(
            WindTableCollection windTables,
            Route route,
            int cruizeLevel, 
            int tas)
        {
            double AirDisToDest = 0.0;
            double GrdDisToDest = 0.0;

            var node = route.First;

            while (node != route.Last)
            {
                var t = GetAirDisGrdDis(
                    windTables,
                    node.Value.Waypoint.LatLon,
                    node.Next.Value.Waypoint.LatLon,
                    tas,
                    cruizeLevel);

                AirDisToDest += t.AirDis;
                GrdDisToDest += t.GrdDis;
                node = node.Next;
            }

            return RoundToInt(tas * (GrdDisToDest / AirDisToDest - 1.0));
        }

        public static AirGrdDistance GetAirDisGrdDis(
            WindTableCollection windTables,
            ICoordinate latlon1, 
            ICoordinate latlon2, 
            int tas,
            double FL)
        {
            //returns airdis and grdDis

            double dis = latlon1.Distance(latlon2);
            var AvgWindCalc = new AvgWindCalculator(windTables, tas, FL);
            double avgWind = AvgWindCalc.GetAvgWind(latlon1, latlon2, 1.0);

            return new AirGrdDistance()
            {
                AirDis = dis * tas / (tas + avgWind),
                GrdDis = dis
            };
        }

        public struct AirGrdDistance
        {
            public double AirDis; public double GrdDis;
        }
    }
}
