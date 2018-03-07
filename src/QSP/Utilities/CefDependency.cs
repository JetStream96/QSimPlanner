using Microsoft.Win32;
using QSP.LibraryExtension;
using System;
using System.Diagnostics;

namespace QSP.Utilities
{
    /// <summary>
    /// Handles dependencies of CefSharp package.
    /// </summary>
    public static class CefDependency
    {
        /// <summary>
        /// Check if VC++ dedistributable is already installed.
        /// Returns false if failed.
        /// </summary>
        public static bool VCppAlreadyInstalled()
        {
            // Registry value from https://stackoverflow.com/questions/12206314/detect-if-visual-c-redistributable-for-visual-studio-2012-is-installed
            var reg = Registries.TryGetRegistry(RegistryHive.LocalMachine,
                @"SOFTWARE\Classes\Installer\Dependencies\{f65db027-aff3-4070-886a-0d87064aabb1}",
                "Version",
                RegistryView.Registry64);

            return reg != null;
        }

        /// <summary>
        /// Starts the installer of VC++ redistributable. Then waits for the installation
        /// to finish.
        /// </summary>
        public static void InstallVCpp()
        {
            var info = new ProcessStartInfo()
            {
                WorkingDirectory = ".",
                FileName = "vcredist_x86.exe",
                Arguments = "/install /passive /norestart"
                // Arguments from http://asawicki.info/news_1597_installing_visual_c_redistributable_package_from_command_line.html
            };

            if (Environment.OSVersion.Version.Major >= 6) info.Verb = "runas";
            Process.Start(info).WaitForExit();
        }

        public static void InstallIfNeeded()
        {
            if (!VCppAlreadyInstalled()) InstallVCpp();
        }
    }
}
