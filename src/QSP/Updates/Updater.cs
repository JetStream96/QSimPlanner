using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Xml.Linq;

namespace QSP.Updates
{
    public class Updater
    {
        public static readonly string FileUri = @"";

        public bool IsUpdating { get; private set; }

        public Updater() { }

        public void Update()
        {

        }

        /// <summary>
        /// Returns the uri of zip file for the newer version.
        /// If no newer version is available, return null.
        /// </summary>
        public static UpdateInfo GetUpdateFileUri()
        {
            using (var client = new WebClient())
            {
                var info = client.DownloadString(FileUri);
                var root = XDocument.Parse(info).Root;
                var version = root.Element("version").Value;

                if (!IsNewerVersion(version)) return null;
                var uri = new Uri(root.Element("uri").Value);
                return new UpdateInfo() { Uri = uri, Version = version };
            }
        }

        public static void Install(UpdateInfo info)
        {
            var zipFilePath = info.Version + ".zip";
            var extractDir = Path.Combine("..", info.Version);

            using (var client = new WebClient())
            {
                client.DownloadFile(info.Uri, zipFilePath);
            }

            ZipFile.ExtractToDirectory(zipFilePath, extractDir);
        }

        public class UpdateInfo
        {
            public Uri Uri; public string Version;
        }

        // Input format should be 1.0.0
        private static bool IsNewerVersion(string version)
        {
            var ver = Assembly.GetEntryAssembly().GetName().Version;
            var current = new Version(ver.Major, ver.Minor, ver.Build);
            return Version.Parse(version) > current;
        }
    }
}
