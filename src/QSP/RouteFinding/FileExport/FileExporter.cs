using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.FileExport.Providers;
using QSP.RouteFinding.Routes;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// <returns></returns>
        public IEnumerable<Status> Export()
        {
            _reports = new List<Status>();

            foreach (var i in commands)
            {
                if (i.Enabled == false)
                {
                    continue;
                }

                try
                {
                    var provider = ProviderFactory.GetProvider(
                        i.ProviderType,
                        route,
                        airports);

                    string filePath = FileNameGenerator.Generate(
                        i.Directory,
                        GetFileName(),
                        (n) => n.ToString().PadLeft(2, '0'),
                        i.Extension);

                    File.WriteAllText(filePath, provider.GetExportText());

                    _reports.Add(new Status(filePath, true, ""));
                }
                catch (Exception ex)
                {
                    LoggerInstance.WriteToLog(ex);
                    _reports.Add(new Status(i.Directory, false, ex.Message));
                }
            }

            return _reports;
        }

        private string GetFileName()
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

            public Status(string FilePath, bool Successful, string Message)
            {
                this.FilePath = FilePath;
                this.Successful = Successful;
                this.Message = Message;
            }
        }
    }
}
