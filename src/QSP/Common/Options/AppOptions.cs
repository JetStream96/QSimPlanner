using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.Common.Options
{
    public class AppOptions
    {
        public string NavDataLocation { get; private set; }
        public bool PromptBeforeExit { get; private set; }
        public bool AutoDLTracks { get; private set; }
        public bool AutoDLWind { get; private set; }
        public bool EnableWindOptimizedRoute { get; private set; }
        public bool HideDctInRoute { get; private set; }
        public bool ShowTrackIdOnly { get; private set; }
        public int UpdateFrequency { get; private set; }
        private Dictionary<string, ExportCommand> _exportCommands;

        public IReadOnlyDictionary<string, ExportCommand> ExportCommands
        {
            get { return _exportCommands; }
        }

        public AppOptions(
            string NavDataLocation,
            bool PromptBeforeExit,
            bool AutoDLTracks,
            bool AutoDLWind,
            bool EnableWindOptimizedRoute,
            bool HideDctInRoute,
            bool ShowTrackIdOnly,
            int UpdateFrequency,
            Dictionary<string, ExportCommand> ExportCommands)
        {
            this.NavDataLocation = NavDataLocation;
            this.PromptBeforeExit = PromptBeforeExit;
            this.AutoDLTracks = AutoDLTracks;
            this.AutoDLWind = AutoDLWind;
            this.EnableWindOptimizedRoute = EnableWindOptimizedRoute;
            this.HideDctInRoute = HideDctInRoute;
            this.ShowTrackIdOnly = ShowTrackIdOnly;
            this.UpdateFrequency = UpdateFrequency;
            this._exportCommands = ExportCommands;
        }
                
        public XElement Serialize()
        {
            var elem = _exportCommands.Values.Select(
                v => v.Serialize("Item"));
            var exportOptions = new XElement("ExportOptions", elem);

            return new XElement("AppOptions", new XElement[]
            {
                NavDataLocation.Serialize("DatabasePath"),
                PromptBeforeExit.Serialize("PromptBeforeExit"),
                AutoDLTracks.Serialize("AutoDLNats"),
                AutoDLWind.Serialize("AutoDLWind"),
                EnableWindOptimizedRoute.Serialize("WindOptimizedRoute"),
                HideDctInRoute.Serialize("HideDctInRoute"),
                ShowTrackIdOnly.Serialize("ShowTrackIdOnly"),
                UpdateFrequency.Serialize("UpdateFrequency"),
                exportOptions
            });
        }

        public static AppOptions Deserialize(XElement item)
        {
            var d = Default;

            Action[] actions =
            {
                () => d.NavDataLocation = item.GetString("DatabasePath"),
                () => d.PromptBeforeExit = item.GetBool("PromptBeforeExit"),
                () => d.AutoDLTracks = item.GetBool("AutoDLNats"),
                () => d.AutoDLWind = item.GetBool("AutoDLWind"),
                () => d.EnableWindOptimizedRoute =
                        item.GetBool("WindOptimizedRoute"),
                () => d.HideDctInRoute = item.GetBool("HideDctInRoute"),
                () => d.ShowTrackIdOnly = item.GetBool("ShowTrackIdOnly"),
                () => d.UpdateFrequency = item.GetInt("UpdateFrequency"),
                () => d._exportCommands =
                        item.Element("ExportOptions")
                            .Elements("Item")
                            .Select(e => ExportCommand.Deserialize(e))
                            .ToDictionary(c => c.ProviderType.ToString())
            };

            foreach (var a in actions)
            {
                IgnoreExceptions(a);
            }

            return d;
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
                    true,
                    false,
                    false,
                    0,
                    new Dictionary<string, ExportCommand>());
            }
        }
    }
}
