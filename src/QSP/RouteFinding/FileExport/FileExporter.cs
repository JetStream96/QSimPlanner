using QSP.Common;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.FileExport.Providers;
using QSP.RouteFinding.Routes;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using QSP.LibraryExtension;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.RouteFinding.FileExport
{
    public class FileExporter
    {
        private readonly Route route;
        private readonly AirportManager airports;
        private readonly IEnumerable<ExportCommand> commands;

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
            var cmdToExport = commands.Where(i => i.Enabled).ToList();
            var nameBase = GetFileNameBase().RemoveIllegalChars();

            TryCreateDirectories();
            int num = FileNameNum(cmdToExport, nameBase);
            if (num != -1) return cmdToExport.Select(c => Export(nameBase, c, num));

            throw new NoFileNameAvailException("No suitable file name can be generated.");
        }

        // Find a file name which allows us to export without name conflicts.
        private int FileNameNum(IReadOnlyList<ExportCommand> cmdToExport, string nameBase)
        {
            const int maxAttemptCount = 10000;
            for (int i = 1; i <= maxAttemptCount; i++)
            {
                if (cmdToExport.All(c => !FileExist(nameBase, c, i))) return i;
            }

            return -1;
        }

        private Status Export(string nameBase, ExportCommand c, int i)
        {
            var fileName = GetFileFullPath(nameBase, c, i);
            var provider = ProviderFactory.GetProvider(c.ProviderType, route, airports);

            try
            {
                // Although the file name has been checked to have no conflict, if the user choose
                // to export multiple files to the same folder, the file names can still collide.
                var newName = File.Exists(fileName)
                    ? GenerateFileName(fileName, c)
                    : fileName;

                File.WriteAllText(newName, provider.GetExportText());
                return new Status(fileName, true, "", false);
            }
            catch (Exception ex)
            {
                LoggerInstance.Log(ex);
                var mayBePermissionIssue = ex is UnauthorizedAccessException ||
                    ex is SecurityException;

                return new Status(c.Directory, false, ex.Message, mayBePermissionIssue);
            }
        }

        private static string GenerateFileName(string fileName, ExportCommand c)
        {
            return FileNameGenerator.Generate(c.Directory.RemoveIllegalChars(),
                fileName, c.Extension, n => n.ToString(), 1);
        }

        private static bool FileExist(string nameBase, ExportCommand cmd, int n)
        {
            var filePath = GetFileFullPath(nameBase, cmd, n);
            return File.Exists(filePath);
        }

        private void TryCreateDirectories()
        {
            foreach (var i in commands)
            {
                IgnoreException(() => Directory.CreateDirectory(i.Directory));
            }
        }

        private static string GetFileFullPath(string nameBase, ExportCommand cmd, int n)
        {
            var fileName = nameBase + n.ToString().PadLeft(2, '0') + cmd.Extension;
            return Path.Combine(cmd.Directory.RemoveIllegalChars(), fileName.RemoveIllegalChars());
        }

        private string GetFileNameBase()
        {
            var orig = route.FirstWaypoint.ID.Substring(0, 4);
            var dest = route.LastWaypoint.ID.Substring(0, 4);
            return orig.ToUpper() + dest.ToUpper();
        }

        public class Status
        {
            public string FilePath { get; }
            public bool Successful { get; }
            public string Message { get; }
            public bool MayBePermissionIssue { get; }

            public Status(string FilePath, bool Successful, string Message,
                bool MayBePermissionIssue)
            {
                this.FilePath = FilePath;
                this.Successful = Successful;
                this.Message = Message;
                this.MayBePermissionIssue = MayBePermissionIssue;
            }
        }
    }
}
