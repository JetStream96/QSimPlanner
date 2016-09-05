using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using static QSP.Utilities.LoggerInstance;
using static QSP.Utilities.ExceptionHelpers;
using static QSP.Updates.Utilities;

namespace QSP.Updates
{
    public class Updater
    {
        public static readonly string FileUri = @"F:\Programming\Projects\QSimPlanner\Updates\info.xml";
        public bool IsUpdating { get; private set; } = false;

        public Updater() { }

        public UpdateStatus Update()
        {
            IsUpdating = true;
            var status = DoUpdateActions();
            IsUpdating = false;
            return status;
        }

        private UpdateStatus DoUpdateActions()
        {
            UpdateInfo info = null;

            try
            {
                info = GetUpdateFileUri();

                if (info == null)
                {
                    return new UpdateStatus(
                        true, "Current version is up to date.");
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                return new UpdateStatus(
                    false, "Failed to obtain update info.");
            }

            try
            {
                Install(info);
                CreateXml(info.Version);
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                return new UpdateStatus(false, "Failed to install update.");
            }

            return new UpdateStatus(true,
                $"Successfully updated to version {info.Version}. " + 
                "Changes will be in effect after restart.");
        }

        private static void CreateXml(string version)
        {
            var elem = new XElement("PostUpdateActionCompleted", "0");
            var root = new XElement("root", elem);
            var doc = new XDocument(root);
            var path = Path.Combine("..", version, "updater.xml");
            File.WriteAllText(path, doc.ToString());
        }

        public class UpdateStatus
        {
            public bool Success; public string Message;

            public UpdateStatus(bool Success, string Message)
            {
                this.Success = Success;
                this.Message = Message;
            }
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
            IgnoreExceptions(() => File.Delete(zipFilePath));
            UpdateXmlAndDeleteOldVersion(info.Version);
        }

        private static void UpdateXmlAndDeleteOldVersion(string version)
        {
            var doc = GetVersionXDoc();
            var root = doc.Root;
            var backup = root.Element("current").Value;
            IgnoreExceptions(() => 
            Directory.Delete(root.Element("backup").Value));
            root.Element("current").Value = version;
            root.Element("backup").Value = backup;
            File.WriteAllText(VersionXmlPath, doc.ToString());
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
