using System.Text.RegularExpressions;

namespace QSP.AviationTools.Coordinates
{
    /// <summary>
    /// e.g. 
    /// 2100S13530E = South 21 deg, East 135 deg 30 min.
    /// 2100.3S13530.7E
    /// </summary>
    public static class FormatDegMinNoSymbol
    {
        /// <summary>
        /// Returns null if the format does not match.
        /// </summary>
        public static LatLon Parse(string s)
        {
            var match = Regex.Match(s, @"([\d\.]+[NS])([\d\.]+[EW])");
            if (!match.Success) return null;

            try
            {
                var a = match.Groups[1].Value;
                var b = match.Groups[2].Value;

                var (a0, a1, a2) = (
                    a.Substring(0, 2),
                    a.Substring(2, a.Length - 3),
                    a[a.Length - 1]);

                var (b0, b1, b2) = (
                    b.Substring(0, 3),
                    b.Substring(3, b.Length - 4),
                    b[b.Length - 1]);

                var lat = (int.Parse(a0) + (a1 == "" ? 0 : double.Parse(a1)) / 60.0)
                           * (a2 == 'S' ? -1 : 1);
                var lon = (int.Parse(b0) + (b1 == "" ? 0 : double.Parse(b1)) / 60.0)
                           * (b2 == 'W' ? -1 : 1);

                if (-90.0 <= lat && lat <= 90.0 && -180.0 <= lon && lon <= 180.0)
                {
                    return new LatLon(lat, lon);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}