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

        // @Throws
        // The versions are strings of the format major.minor.build.
        // Backup version is empty string if the application was never updated.
        // Do NOT use reflection to get the current version so that it is easier to test the
        // updater system.
        public static VersionInfo GetVersions()
        {
            var root = GetVersionXDoc().Root;

            return new VersionInfo()
            {
                Backup = root.Element("backup").Value,
                Current = root.Element("current").Value
            };
        }

        // @Throws
        public static XDocument GetVersionXDoc()
        {
            return XDocument.Load(VersionXmlPath);
        }

        public class VersionInfo { public string Backup, Current; }
    }
}
