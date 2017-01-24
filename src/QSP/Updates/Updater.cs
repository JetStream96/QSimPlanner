using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static QSP.Utilities.LoggerInstance;
using static QSP.Utilities.ExceptionHelpers;
using static QSP.Updates.Utilities;

namespace QSP.Updates
{
    public class Updater
    {
        public bool IsUpdating { get; private set; } = false;

        public Updater() { }

        public UpdateResult Update()
        {
            IsUpdating = true;
            var status = DoUpdateActions();
            IsUpdating = false;
            return status;
        }

        private UpdateResult DoUpdateActions()
        {
            UpdateInfo info = null;

            try
            {
                info = GetUpdateFileUri();

                if (info == null)
                {
                    return new UpdateResult(Status.AlreadyUpToDate,
                        "Current version is up to date.");
                }
            }
            catch (Exception ex)
            {
                Log(ex);
                return new UpdateResult(Status.Failed, "Failed to obtain update info.");
            }

            try
            {
                Install(info);
                MarkForPostUpdateActions(info.Version);

                // This has to be the last step, after all operations succeeds.
                UpdateXmlAndDeleteOldVersion(info.Version);
            }
            catch (Exception ex)
            {
                Log(ex);
                return new UpdateResult(Status.Failed, "Failed to install update.");
            }

            return new UpdateResult(Status.Success,
                $"Successfully updated to version {info.Version}. " +
                "Changes will be in effect after restart.");
        }

        private static void MarkForPostUpdateActions(string version)
        {
            var path = Path.Combine("..", version, "updater.xml");
            var doc = XDocument.Load(path);
            doc.Root.Element("PostUpdateActionCompleted").Value = "0";
            File.WriteAllText(path, doc.ToString());
        }

        public enum Status
        {
            Success,
            AlreadyUpToDate,
            Failed
        }

        public class UpdateResult
        {
            public Status Status { get; }
            public string Message { get; }

            public UpdateResult(Status Status, string Message)
            {
                this.Status = Status;
                this.Message = Message;
            }
        }

        /// <summary>
        /// Returns the uri of zip file for the newer version.
        /// If no newer version is available, return null. May throw exception.
        /// </summary>
        public static UpdateInfo GetUpdateFileUri()
        {
            var root = XDocument.Parse(GetInfoFileContent()).Root;
            var version = root.Element("version").Value;

            var currentVer = GetVersions().Current;
            if (Version.Parse(version) <= Version.Parse(currentVer)) return null;
            var uri = new Uri(root.Element("uri").Value);
            return new UpdateInfo() { Uri = uri, Version = version };
        }

        private static string GetInfoFileContent()
        {
            using (var client = new WebClient())
            {
                foreach (var i in FileUris())
                {
                    try
                    {
                        return client.DownloadString(i);
                    }
                    catch { }
                }
            }

            throw new WebException("Cannot obtain update info file.");
        }

        private static IEnumerable<string> FileUris()
        {
            var doc = XDocument.Load("updater.xml");
            var elem = doc.Root.Element("InfoFiles").Elements("Uri");
            return elem.Select(e => e.Value);
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
            IgnoreException(() => File.Delete(zipFilePath));
        }

        private static void UpdateXmlAndDeleteOldVersion(string version)
        {
            var doc = GetVersionXDoc();
            var root = doc.Root;
            var backup = root.Element("current").Value;
            IgnoreException(() => Directory.Delete(root.Element("backup").Value));
            root.Element("current").Value = version;
            root.Element("backup").Value = backup;
            File.WriteAllText(VersionXmlPath, doc.ToString());
        }

        public class UpdateInfo
        {
            public Uri Uri; public string Version;
        }
    }
}
