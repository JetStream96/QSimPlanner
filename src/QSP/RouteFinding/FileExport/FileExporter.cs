using QSP.Common;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
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
        private readonly Route route;
        private readonly AirportManager airports;
        private readonly IEnumerable<ExportCommand> commands;
        private readonly Func<AppOptions> options;

        public FileExporter(
            Route route,
            AirportManager airports,
            IEnumerable<ExportCommand> commands,
            Func<AppOptions> options)
        {
            this.route = route;
            this.airports = airports;
            this.commands = commands;
            this.options = options;
        }

        /// <summary>
        /// Returns the error messages of the operation.
        /// </summary>
        /// <exception cref="NoFileNameAvailException"></exception>
        public IEnumerable<Status> Export()
        {
            var cmdToExport = commands.Where(i => i.Enabled).ToList();
            var nameBase = GetFileNameBase().RemoveIllegalFileNameChars();

            TryCreateDirectories(cmdToExport);
            int num = FileNameNum(cmdToExport, nameBase);
            if (num != -1) return cmdToExport.Select(c => Export(nameBase, c, num));

            throw new NoFileNameAvailException("No suitable file name can be generated.");
        }

        // Find a file name which allows us to export without name conflicts.
        private int FileNameNum(IReadOnlyList<ExportCommand> cmdToExport, string nameBase)
        {
            const int maxAttemptCount = 1000;
            for (int i = 1; i <= maxAttemptCount; i++)
            {
                if (cmdToExport.All(c => !FileExist(nameBase, c, i))) return i;
            }

            return -1;
        }

        private Status Export(string nameBase, ExportCommand c, int i)
        {
            try
            {
                var fileName = GetFileFullPath(nameBase, c, i);

                // Although the file name has been checked to have no conflict, if the user choose
                // to export multiple files to the same folder, the file names can still collide.
                var newName = File.Exists(fileName)
                    ? GenerateFileName(nameBase + i.ToString().PadLeft(2, '0'), c)
                    : fileName;

                File.WriteAllText(newName,
                    Providers.Types.GetExportText(c.ProviderType, route, airports));
                return new Status(newName, true, "", false);
            }
            catch (Exception ex)
            {
                LoggerInstance.Log(ex);
                var mayBePermissionIssue = ex is UnauthorizedAccessException ||
                    ex is SecurityException;

                return new Status(c.CustomDirectory, false, ex.Message, mayBePermissionIssue);
            }
        }

        private static string GenerateFileName(string nameBase, ExportCommand c)
        {
            return FileNameGenerator.Generate(c.CustomDirectory.RemoveIllegalPathChars(),
                nameBase, c.Extension, n => "_" + n.ToString(), 1);
        }

        private bool FileExist(string nameBase, ExportCommand cmd, int n)
        {
            return DefaultIfThrows(() =>
            {
                var filePath = GetFileFullPath(nameBase, cmd, n);
                return File.Exists(filePath);
            }, false);
        }

        /// Returned path may be null or not exist.
        private string ExportDirectory(ExportCommand c) =>
            Providers.Types.ExportDirectory(c.DefaultSimulator, c, options());

        private void TryCreateDirectories(IEnumerable<ExportCommand> enabledCommands)
        {
            foreach (var i in enabledCommands)
            {
                IgnoreException(() => Directory.CreateDirectory(ExportDirectory(i)));
            }
        }

        // May throw exception. Returned path may not exist.
        private string GetFileFullPath(string nameBase, ExportCommand cmd, int n)
        {
            var fileName = nameBase + n.ToString().PadLeft(2, '0') + cmd.Extension;
            return Path.Combine(ExportDirectory(cmd).RemoveIllegalPathChars(),
                fileName.RemoveIllegalFileNameChars());
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
