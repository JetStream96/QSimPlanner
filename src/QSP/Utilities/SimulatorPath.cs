using Microsoft.Win32;
using QSP.LibraryExtension;

namespace QSP.Utilities
{
    public static class SimulatorPath
    {
        // All methods in this class returns null if the registry cannot be found.

        public static string FsxPath()
        {
            var lm = Registries.TryGetRegistry(
                RegistryHive.LocalMachine,
                @"SOFTWARE\Microsoft\microsoft games\flight simulator\10.0",
                "SetupPath",
                RegistryView.Registry32);   // FSX is 32-bit.

            var cu = Registries.TryGetRegistry(
                RegistryHive.CurrentUser,
                @"SOFTWARE\Microsoft\microsoft games\flight simulator\10.0",
                "AppPath",
                RegistryView.Registry32);

            return lm ?? cu;
        }

        public static string FsxSteamPath()
        {
            var lm = Registries.TryGetRegistry(
                RegistryHive.LocalMachine,
                @"Software\Dovetail Games\FSX\10.0",
                "Install_Path",
                RegistryView.Registry32);   // FSX:SE is 32-bit.

            var cu = Registries.TryGetRegistry(
                RegistryHive.CurrentUser,
                @"Software\Microsoft\Microsoft Games\Flight Simulator - Steam Edition\10.0",
                "AppPath",
                RegistryView.Registry32);   // FSX:SE is 32-bit.

            return lm ?? cu;
        }

        public static string P3Dv4Path()
        {
            var lm = Registries.TryGetRegistry(
                RegistryHive.LocalMachine,
                @"SOFTWARE\Lockheed Martin\Prepar3D v4",
                "SetupPath",
                RegistryView.Registry64);   // P3Dv4 is 64-bit.

            var cu = Registries.TryGetRegistry(
                RegistryHive.CurrentUser,
                @"SOFTWARE\Lockheed Martin\Prepar3D v4",
                "AppPath",
                RegistryView.Registry64);   // P3Dv4 is 64-bit.

            return lm ?? cu;
        }

        public static string P3Dv3Path()
        {
            var lm = Registries.TryGetRegistry(
                RegistryHive.LocalMachine,
                @"Software\Lockheed Martin\Prepar3d v3",
                "SetupPath",
                RegistryView.Registry32);   // P3Dv3 is 32-bit.

            var cu = Registries.TryGetRegistry(
                RegistryHive.CurrentUser,
                @"Software\Lockheed Martin\Prepar3D v3",
                "AppPath",
                RegistryView.Registry32);   // P3Dv3 is 32-bit.

            return lm ?? cu;
        }

        public static string P3Dv2Path()
        {
            var lm = Registries.TryGetRegistry(
                RegistryHive.LocalMachine,
                @"Software\Lockheed Martin\Prepar3d v2",
                "SetupPath",
                RegistryView.Registry32);   // P3Dv2 is 32-bit.

            var cu = Registries.TryGetRegistry(
                RegistryHive.CurrentUser,
                @"Software\Lockheed Martin\Prepar3D v2",
                "AppPath",
                RegistryView.Registry32);   // P3Dv2 is 32-bit.

            return lm ?? cu;
        }

        public static string P3Dv1Path()
        {
            var lm = Registries.TryGetRegistry(
                RegistryHive.LocalMachine,
                @"Software\LockheedMartin\Prepar3d",
                "SetupPath",
                RegistryView.Registry32);   // P3Dv1 is 32-bit.

            var cu = Registries.TryGetRegistry(
                RegistryHive.CurrentUser,
                @"Software\LockheedMartin\Prepar3D",
                "AppPath",
                RegistryView.Registry32);   // P3Dv1 is 32-bit.

            return lm ?? cu;
        }
    }
}