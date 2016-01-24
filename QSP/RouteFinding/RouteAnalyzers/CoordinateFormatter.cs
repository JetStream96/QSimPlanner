using QSP.AviationTools.Coordinates;
using System;
using static QSP.LibraryExtension.StringParser.Utilities;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // Split an string representing a route into an array of strings, which is then processed by various 
    // route analyzers.
    //
    // This is mainly used to convert all supported lat/lon formats to decimal format.
    //

    public class CoordinateFormatter
    {
        private string route;

        public CoordinateFormatter(string route)
        {
            this.route = route;
        }

        public string[] Split()
        {
            var s = route.Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < s.Length; i++)
            {
                LatLon coord;

                if (Format5Letter.TryReadFrom5LetterFormat(s[i], out coord) ||
                    Format7Letter.TryReadFrom7LetterFormat(s[i], out coord))
                {
                    s[i] = coord.ToDecimalFormat();
                }
            }
            return s;
        }
    }
}
