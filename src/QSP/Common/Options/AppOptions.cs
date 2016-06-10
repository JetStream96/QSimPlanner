using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace QSP.Common.Options
{
    public class AppOptions
    {
        // TODO: make immutable or at least better encapsulation
        public string NavDataLocation { get; set; }
        public bool PromptBeforeExit { get; set; }
        public bool AutoDLTracks { get; set; }
        public bool AutoDLWind { get; set; }
        public Dictionary<string, ExportCommand> ExportCommands { get; set; }

        public AppOptions(
            string NavDataLocation,
            bool PromptBeforeExit,
            bool AutoDLTracks,
            bool AutoDLWind,
            Dictionary<string, ExportCommand> ExportCommands)
        {
            this.NavDataLocation = NavDataLocation;
            this.PromptBeforeExit = PromptBeforeExit;
            this.AutoDLTracks = AutoDLTracks;
            this.AutoDLWind = AutoDLWind;
            this.ExportCommands = ExportCommands;
        }

        // TODO: exceptions?
        public AppOptions(XDocument xmlFile)
        {
            var root = xmlFile.Root;

            NavDataLocation = root.Element("DatabasePath").Value;
            PromptBeforeExit = bool.Parse(
                root.Element("PromptBeforeExit").Value);
            AutoDLTracks = bool.Parse(root.Element("AutoDLNats").Value);
            AutoDLWind = bool.Parse(root.Element("AutoDLWind").Value);

            var exports = root.Element("ExportOptions");

            ExportCommands = new Dictionary<string, ExportCommand>();

            foreach (var i in exports.Elements())
            {
                var type = (ProviderType)Enum.Parse(
                    typeof(ProviderType),
                    i.Element("Type").Value);

                var cmd = new ExportCommand(
                    type,
                    i.Element("Path").Value,
                    bool.Parse(i.Element("Enabled").Value));

                ExportCommands.Add(i.Name.LocalName, cmd);
            }
        }

        public XElement ToXml()
        {
            var exports = ExportCommands.Select(entry =>
            {
                var command = entry.Value;

                return
                new XElement(
                    entry.Key.ToString(),
                    new XElement[] {
                        new XElement("Type", command.ProviderType.ToString()),
                        new XElement("Enabled", command.Enabled.ToString()),
                        new XElement("Path", command.Directory)});
            });

            var exportOptions = new XElement("ExportOptions", exports);

            return new XElement("AppOptions", new XElement[] {
                new XElement("DatabasePath", NavDataLocation),
                new XElement("PromptBeforeExit", PromptBeforeExit.ToString()),
                new XElement("AutoDLNats", AutoDLTracks.ToString()),
                new XElement("AutoDLWind", AutoDLWind.ToString()),
                exportOptions});
        }

        public ExportCommand GetExportCommand(string key)
        {
            return ExportCommands[key];
        }

        public static AppOptions Default
        {
            get
            {
                return new AppOptions(
                    "",
                    true,
                    true,
                    true,
                    new Dictionary<string, ExportCommand>());
            }
        }
    }
}
