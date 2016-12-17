using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using static QSP.Utilities.SimulatorPath;

namespace QSP.NavData
{
    // Gets the navData folder path from registry entries.

    public static class NavDataPath
    {


        private static string[] SimulatorPaths()
        {
            return new[]
            {
                FsxPath(),
                FsxSteamPath(),
                P3Dv3Path(),
                P3Dv2Path(),
                P3Dv1Path()
            };
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