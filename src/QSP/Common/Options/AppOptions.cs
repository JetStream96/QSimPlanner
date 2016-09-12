using QSP.RouteFinding.FileExport;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using QSP.LibraryExtension.XmlSerialization;
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
        public bool AutoUpdate { get; private set; }
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
            bool AutoUpdate,
            Dictionary<string, ExportCommand> ExportCommands)
        {
            this.NavDataLocation = NavDataLocation;
            this.PromptBeforeExit = PromptBeforeExit;
            this.AutoDLTracks = AutoDLTracks;
            this.AutoDLWind = AutoDLWind;
            this.EnableWindOptimizedRoute = EnableWindOptimizedRoute;
            this.HideDctInRoute = HideDctInRoute;
            this.ShowTrackIdOnly = ShowTrackIdOnly;
            this.AutoUpdate = AutoUpdate;
            this._exportCommands = ExportCommands;
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
                    true,
                    new Dictionary<string, ExportCommand>());
            }
        }
        
        public class Serializer : IXSerializer<AppOptions>
        {
            public XElement Serialize(AppOptions a, string name)
            {
                var elem = a._exportCommands.Select(kv =>
                    new XElement("KeyValuePair",
                        kv.Key.Serialize("Key"),
                        kv.Value.Serialize("Value")));
                var exportOptions = new XElement("ExportOptions", elem);

                return new XElement(name, new XElement[]
                {
                    a.NavDataLocation.Serialize("DatabasePath"),
                    a.PromptBeforeExit.Serialize("PromptBeforeExit"),
                    a.AutoDLTracks.Serialize("AutoDLTracks"),
                    a.AutoDLWind.Serialize("AutoDLWind"),
                    a.EnableWindOptimizedRoute.Serialize("WindOptimizedRoute"),
                    a.HideDctInRoute.Serialize("HideDctInRoute"),
                    a.ShowTrackIdOnly.Serialize("ShowTrackIdOnly"),
                    a.AutoUpdate.Serialize("AutoUpdate"),
                    exportOptions
                });
            }

            public AppOptions Deserialize(XElement item)
            {
                var d = Default;

                Action[] actions =
                {
                () => d.NavDataLocation = item.GetString("DatabasePath"),
                () => d.PromptBeforeExit = item.GetBool("PromptBeforeExit"),
                () => d.AutoDLTracks = item.GetBool("AutoDLTracks"),
                () => d.AutoDLWind = item.GetBool("AutoDLWind"),
                () => d.EnableWindOptimizedRoute =
                        item.GetBool("WindOptimizedRoute"),
                () => d.HideDctInRoute = item.GetBool("HideDctInRoute"),
                () => d.ShowTrackIdOnly = item.GetBool("ShowTrackIdOnly"),
                () => d.AutoUpdate = item.GetBool("AutoUpdate"),
                () => d._exportCommands =
                    item.Element("ExportOptions")
                        .Elements("KeyValuePair")
                        .ToDictionary(
                            e => e.GetString("Key"),
                            e => ExportCommand.Deserialize(e.Element("Value")))
            };

                foreach (var a in actions)
                {
                    IgnoreExceptions(a);
                }

                return d;
            }
        }

    }
}
