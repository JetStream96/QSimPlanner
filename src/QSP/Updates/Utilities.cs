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
        
        // Backup is an empty string if the element does not exist.
        // May throw exception.
        // The versions are strings of the format major.minor.build.
        // Backup version may be empty string if the application was never updated.
        public static VersionInfo GetVersions()
        {
            var root = GetVersionXDoc().Root;

            return new VersionInfo()
            {
                Backup = root.Element("backup").Value,
                Current = root.Element("current").Value
            };
        }

        // May throw exception.
        public static XDocument GetVersionXDoc()
        {
            return XDocument.Load(VersionXmlPath);
        }

        public class VersionInfo { public string Backup, Current; }
    }
}
