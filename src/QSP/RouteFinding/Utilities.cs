using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Containers;
using System.Collections.Generic;
using static QSP.MathTools.Utilities;

namespace QSP.RouteFinding
{

    public static class Utilities
    {
        private static string[] correctFixType = { "IF", "DF", "TF", "FD", "CF" };
        //, "AF", "RF", "CD", "FA", "FC", "FM", "VD", "PI", "HF", "HA", "HM"}
        //this lists all kinds of fixes with coordinates located in t(2) and t(3)

        /// <summary>
        /// Determines whether the input string is a valid rwy identifier.
        /// </summary>
        public static bool IsRwyIdent(string str)
        {
            int i = 0;

            if (str.Length == 2 && int.TryParse(str, out i) && (i > 0 && i <= 36))
            {
                return true;
            }
            else if (str.Length == 3 &&
                     (str[2] == 'L' || str[2] == 'R' || str[2] == 'C') &&
                     int.TryParse(str.Substring(0, 2), out i) &&
                     (i > 0 && i <= 36))
            {
                return true;
            }
            return false;
        }

        public static bool HasCorrds(string fixType)
        {
            foreach (var i in correctFixType)
            {
                if (i == fixType)
                {
                    return true;
                }
            }
            return false;
        }

        public static double GetTotalDistance(List<LatLon> latLons)
        {
            //in nm
            if (latLons.Count < 2)
            {
                return 0.0;
            }
            double dis = 0.0;

            for (int i = 0; i < latLons.Count - 1; i++)
            {
                dis += GreatCircleDistance(latLons[i], latLons[i + 1]);
            }
            return dis;
        }

        public static double GetTotalDistance<T>(T wpts) 
            where T : IReadOnlyCollection<Waypoint>, IReadOnlyList<Waypoint>
        {
            //in nm
            if (wpts.Count < 2)
            {
                return 0.0;
            }
            double dis = 0.0;

            for (int i = 0; i < wpts.Count - 1; i++)
            {
                dis += GreatCircleDistance(wpts[i].Lat, wpts[i].Lon,
                                           wpts[i + 1].Lat, wpts[i + 1].Lon);
            }
            return dis;
        }        
    }
}

