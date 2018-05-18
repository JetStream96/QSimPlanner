using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System.Linq;
using static QSP.LibraryExtension.Types;
using static QSP.MathTools.Numbers;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class PmdgWindUplinkProvider
    {
        /// <summary>
        /// Get string of the flight plan to export.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static string GetExportText(Route route, IWxTableCollection w)
        {
            var altitudes = List(6000, 9000, 12000, 18000, 24000,
               30000, 34000, 39000, 44000, 49000);
            return string.Join("\n", route.Select(node =>
            {
                var wpt = node.Waypoint;
                var id = (node == route.First.Value || node == route.Last.Value) ?
                    wpt.ID.Substring(0, 4) :
                    wpt.ID.FormatWaypointId();

                var altitudesStr = altitudes.Select(a => GetWindTemp(w, wpt, a)).ToList();
                return string.Join("\t", List(id).Concat(altitudesStr.Take(5))) + "\n\t" +
                       string.Join("\t", altitudesStr.Skip(5));
            }));
        }

        private static string GetWindTemp(IWxTableCollection w, ICoordinate c, double alt)
        {
            var windUV = w.GetWindUV(c.Lat, c.Lon, alt);
            var wind = Wind.FromUV(windUV);
            return string.Format("{0}@{1}({2})",
                RoundToInt(wind.Direction).ToString().PadLeft(3,'0'),
                RoundToInt(wind.Speed).ToString().PadLeft(2, '0'),
                RoundToInt(w.GetTemp(c.Lat, c.Lon, alt)));
        }
    }
}
