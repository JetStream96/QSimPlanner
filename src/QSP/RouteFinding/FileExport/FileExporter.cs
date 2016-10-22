using QSP.Common;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;
using QSP.RouteFinding.Routes;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.RouteFinding.FileExport
{
    public class FileExporter
    {
        private Route route;
        private AirportManager airports;

        private IEnumerable<ExportCommand> commands;
        private List<Status> _reports;

        public FileExporter(
            Route route,
            AirportManager airports,
            IEnumerable<ExportCommand> commands)
        {
            this.route = route;
            this.airports = airports;
            this.commands = commands;
        }

        /// <summary>
        /// Returns the error messages of the operation.
        /// </summary>
        /// <exception cref="NoFileNameAvailException"></exception>
        public IEnumerable<Status> Export()
        {
            _reports = new List<Status>();
            var cmdToExport = commands.Where(i => i.Enabled);
            var nameBase = GetFileNameBase();
            const int maxAttemptCount = 10000;

            TryCreateDirectories();

            for (int i = 1; i <= maxAttemptCount; i++)
            {
                if (cmdToExport.All(c => !FileExist(nameBase, c, i)))
                {
                    return cmdToExport.Select(c => Export(nameBase, c, i));
                }
            }

            throw new NoFileNameAvailException(
                "No suitable file name can be generated.");
        }

        private Status Export(string nameBase, ExportCommand c, int i)
        {
            var fileName = GetFileFullPath(nameBase, c, i);

            var provider = ProviderFactory.GetProvider(
                c.ProviderType,
                route,
                airports);

            try
            {
                File.WriteAllText(fileName, provider.GetExportText());
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
                var mayBePermissionIssue = ex is UnauthorizedAccessException ||
                    ex is SecurityException;

                return new Status(
                    c.Directory, false, ex.Message, mayBePermissionIssue);
            }

            return new Status(fileName, true, "", false);
        }

        private bool FileExist(string nameBase, ExportCommand cmd, int n)
        {
            var filePath = GetFileFullPath(nameBase, cmd, n);
            return File.Exists(filePath);
        }

        private void TryCreateDirectories()
        {
            foreach (var i in commands)
            {
                IgnoreExceptions(() => Directory.CreateDirectory(i.Directory));
            }
        }

        private static string GetFileFullPath(
            string nameBase, ExportCommand cmd, int n)
        {
            var fileName =
                nameBase + n.ToString().PadLeft(2, '0') + cmd.Extension;
            return Path.Combine(cmd.Directory, fileName);
        }

        private string GetFileNameBase()
        {
            var orig = route.FirstWaypoint.ID.Substring(0, 4);
            var dest = route.LastWaypoint.ID.Substring(0, 4);
            return orig.ToUpper() + dest.ToUpper();
        }

        public class Status
        {
            public string FilePath { get; private set; }
            public bool Successful { get; private set; }
            public string Message { get; private set; }
            public bool MayBePermissionIssue { get; private set; }

            public Status(string FilePath,
                bool Successful, string Message, bool MayBePermissionIssue)
            {
                this.FilePath = FilePath;
                this.Successful = Successful;
                this.Message = Message;
                this.MayBePermissionIssue = MayBePermissionIssue;
            }
        }
    }
}
