using System;
using System.Linq;
using System.Text;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding
{

    static class FlightPlanExport
    {

        private static string latLonToPMDGRteFormat(Vector2D latlon)
        {
            return latLonToPMDGRteFormat(latlon.x, latlon.y);
        }

        private static string latLonToPMDGRteFormat(double lat, double lon)
        {

            string s = null;

            //N
            if (lat > 0)
            {
                s = "N " + lat;
                //S
            }
            else
            {
                s = "S " + (-lat);
            }

            //E
            if (lon > 0)
            {
                s += " E " + lon;
                //W
            }
            else
            {
                s += " W " + (-lon);
            }

            return s;

        }

        /// <summary>
        /// Returns a string represents the PMDG .rte file. SIDs, STARs are not included.
        /// </summary>
        /// <param name="rte"></param>
        public static string GeneratePmdgRteFile(Route rte)
        {

            rte.ExpandNats();

            int numWpts = rte.Waypoints.Count;
            //including dep/arr airports
            string icaoOrig = rte.Waypoints[0].ID.Substring(0, 4);
            string icaoDest = rte.Waypoints.Last().ID.Substring(0, 4);

            StringBuilder result = new StringBuilder();
            appendOrigAirportPart(numWpts, icaoOrig, result);

            string viaStr = null;


            for (int i = 1; i <= numWpts - 2; i++)
            {
                if (i == numWpts - 2 || rte.Via[i] == "DCT")
                {
                    viaStr = "DIRECT";
                }
                else
                {
                    viaStr = rte.Via[i];
                }

                Waypoint w = rte.Waypoints[i];
                result.Append(w.ID + Environment.NewLine + "5" + Environment.NewLine + viaStr + Environment.NewLine + "1 ");
                result.AppendLine(latLonToPMDGRteFormat(w.Lat, w.Lon) + " 0" + Environment.NewLine + "0" + Environment.NewLine + "0" + Environment.NewLine + "0");
                result.AppendLine();

            }

            appendDestAirportPart(icaoDest, result);

            return result.ToString();

        }

        /// <summary>
        /// Appends the ORIG airport part onto the StringBuilder.
        /// </summary>
        /// <param name="icao"></param>
        /// <param name="result"></param>

        private static void appendOrigAirportPart(int numWpts, string icao, StringBuilder result)
        {
            var latLonAlt = RouteFindingCore.AirportList.AirportLatLonAltIcao(icao);

            result.AppendLine("Flight plan is built by QSimPlanner.");
            result.AppendLine();

            result.Append(numWpts + Environment.NewLine + Environment.NewLine);
            result.Append(icao + Environment.NewLine + "1" + Environment.NewLine + "DIRECT" + Environment.NewLine + "1 ");
            result.Append(latLonToPMDGRteFormat(latLonAlt.x, latLonAlt.y) + " " + latLonAlt.z);
            result.AppendLine(Environment.NewLine + "-----" + Environment.NewLine + "1" + Environment.NewLine + "0" + Environment.NewLine + Environment.NewLine + "1" + Environment.NewLine + latLonAlt.z);
            result.Append("-" + Environment.NewLine + "-1000000" + Environment.NewLine + "-1000000" + Environment.NewLine + Environment.NewLine);

        }

        /// <summary>
        /// Appends the DEST airport part onto the StringBuilder.
        /// </summary>
        /// <param name="icao"></param>
        /// <param name="result"></param>

        private static void appendDestAirportPart(string icao, StringBuilder result)
        {
            Vector3D latLonAlt = RouteFindingCore.AirportList.AirportLatLonAltIcao(icao);

            result.Append(icao + Environment.NewLine + "1" + Environment.NewLine + "-" + Environment.NewLine + "1 ");
            result.Append(latLonToPMDGRteFormat(latLonAlt.x, latLonAlt.y) + " " + latLonAlt.z);
            result.AppendLine(Environment.NewLine + "-----" + Environment.NewLine + "0" + Environment.NewLine + "0" + Environment.NewLine + Environment.NewLine + "1" + Environment.NewLine + latLonAlt.z);
            result.Append("-" + Environment.NewLine + "-1000000" + Environment.NewLine + "-1000000" + Environment.NewLine + Environment.NewLine);

        }

    }

}
