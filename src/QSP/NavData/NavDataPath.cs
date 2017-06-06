using System;
using QSP.LibraryExtension;
using System.IO;
using System.Linq;
using QSP.AviationTools.Airac;
using static QSP.Utilities.SimulatorPath;

namespace QSP.NavData
{
    // Gets the navData folder path from registry entries.

    public static class NavDataPath
    {
        /// <summary>
        /// If no valid AIRAC is detected, returns null. This method does not throw exception.
        /// </summary>
        public static AiracInfo DetectNavDataPath()
        {
            var paths = SimulatorPaths()
                .Where(p => p != null)
                .SelectMany(p => new[] { GetNavDataProPath(p), GetNavigraphPath(p) });

            var airacs = paths.Select(d => new AiracInfo()
            {
                Directory = d,
                Period = AiracTools.TryGetAiracCyclePeriod(d)
            }).Where(a => a.Period != null).ToList();

            if (!airacs.Any()) return null;

            // Choose the latest airac cycle.
            return airacs.MaxBy(a => a.Period.Period.End);
        }

        public class AiracInfo
        {
            public string Directory; public AiracPeriod Period;
        }

        // Returns an array of the detected paths.
        // A path may be a string, or null if no path is found or path is invalid.
        // Does not throw exception.
        private static string[] SimulatorPaths()
        {
            Func<string>[] pathGetters =
            {
                FsxPath,
                FsxSteamPath,
                P3Dv4Path,
                P3Dv3Path,
                P3Dv2Path,
                P3Dv1Path
            };

            return pathGetters.Select(f =>
            {
                var path = f();

                if (path != null && !Paths.ContainIllegalPathChar(path)) return path;
                return null;
            }).ToArray();
        }

        // fsxPath can be the path to steam version or P3D versions.
        private static string GetNavigraphPath(string fsxFolder)
        {
            return Path.Combine(fsxFolder, @"aerosoft\Airbus_Fallback\Navigraph");
        }

        private static string GetNavDataProPath(string fsxFolder)
        {
            return Path.Combine(fsxFolder, @"aerosoft\Airbus_Fallback\NavDataPro");
        }
    }
}