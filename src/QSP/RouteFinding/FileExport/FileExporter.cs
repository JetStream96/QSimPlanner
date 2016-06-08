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
        private ManagedRoute route;
        private AirportManager airports;

        private IEnumerable<ExportCommand> commands;
        private List<string> _errors;

        public FileExporter(
            ManagedRoute route,
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
        public IEnumerable<string> Export()
        {
            _errors = new List<string>();

            foreach (var i in commands)
            {
                try
                {
                    var provider = ProviderFactory.GetProvider(
                        i.ProviderType,
                        route,
                        airports);

                    string filePath = FileNameGenerator.Generate(
                        i.Directory,
                        getFileName(),
                        ".rte"); // TODO: extension

                    File.WriteAllText(i.Directory, provider.GetExportText());
                }
                catch (Exception ex)
                {
                    LoggerInstance.WriteToLog(ex);
                    _errors.Add(ex.Message);
                }
            }

            return _errors;
        }      
        
        private string getFileName()
        {
            var orig = route.First.Waypoint.ID.Substring(0, 4);
            var dest = route.Last.Waypoint.ID.Substring(0, 4);
            return orig.ToUpper() + dest.ToUpper();
        }  
    }
}
