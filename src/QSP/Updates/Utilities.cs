using QSP.Common.Options;
using System;
using System.IO;
using System.Xml.Linq;

namespace QSP.Updates
{
    public static class Utilities
    {
        public const string VersionXmlPath = "../version.xml";

        public static string GetFolder(Version ver)
        {
            return Path.Combine("..", ver.ToString());
        }

        /// <summary>
        /// The versions are strings of the format major.minor.build.
        /// Backup version is empty string if the application was never updated.
        /// Do NOT use reflection to get the current version so that it is easier to test the
        /// updater system.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static VersionInfo GetVersions()
        {
            var root = GetVersionXDoc().Root;

            return new VersionInfo()
            {
                Backup = root.Element("backup").Value,
                Current = root.Element("current").Value
            };
        }

        /// <exception cref="Exception"></exception>
        public static XDocument GetVersionXDoc()
        {
            return XDocument.Load(VersionXmlPath);
        }

        public class VersionInfo { public string Backup, Current; }

        // @NoThrow
        /// <summary>
        /// Shows the license only if the current version of application is never run,
        /// and the license text changed from the previous version which the user has.
        /// </summary>
        public static bool ShouldShowLicense()
        {
            try
            {
                if (File.Exists(OptionManager.DefaultPath)) return false;

                var ver = GetVersions();
                var prevTxt = Path.Combine("..", ver.Backup, "LICENSE.txt");
                return File.ReadAllText(prevTxt) != File.ReadAllText("LICENSE.txt");
            }
            catch
            {
                return true;
            }
        }
    }
}
