using QSP.RouteFinding.Routes;
using System;
using System.Linq;
using static QSP.AviationTools.Coordinates.Formatter;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // Split an string representing a route into an array of strings, which is 
    // then processed by various route analyzers.
    //
    // This is mainly used to convert all supported lat/lon formats to 
    // decimal format.
    //
    // 1. Input: The string, consisting of waypoint(WPT), airway(AWY), etc.
    //           These should be seperated by at least one of the following 
    //           char/strings:
    //           (1) space
    //           (2) Tab
    //           (3) LF
    //           (4) CR
    //
    // 2. Not case-sensitive to input string. They get converted to upper 
    //    case before parsing.
    //
    // 3. Format:
    //    (1) Any AWY can be replaced by DCT. The route will be a direct 
    //        between the two waypoints.
    //    (2) Any DCT can be omitted.

    public static class CoordinateFormatter
    {
        private static readonly char[] DelimiterWords = { ' ', '\n', '\r', '\t' };
        
        public static RouteString Split(string route)
        {
            return route
                .ToUpper()
                .Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x != "DCT")
                .Select(TryTransformCoordinate)
                .ToRouteString();
        }
    }
}
