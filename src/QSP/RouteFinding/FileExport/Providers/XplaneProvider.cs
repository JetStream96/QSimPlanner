using System;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.FileExport.Providers
{
    /// <summary>
    /// Implements the "3 version" format. Supports x-plane 8 to 11.
    /// Specs: https://flightplandatabase.com/dev/specification
    /// </summary>
    /// 
    /// For newer format, see https://developer.x-plane.com/?article=flightplan-files-v11-fms-file-format
    public static class XplaneProvider
    {
        /// <summary>
        /// Get string of the flight plan to export.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static string GetExportText(Route route)
        {
            throw new NotImplementedException();

            if (route.Count < 2) throw new ArgumentException();
            var from = route.FirstWaypoint.ID;
            var to = route.LastWaypoint.ID;
            var s = @"I
3 version
1
";
        }
    }
}