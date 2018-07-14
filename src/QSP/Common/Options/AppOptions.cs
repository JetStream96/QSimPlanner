using QSP.LibraryExtension;
using QSP.LibraryExtension.Sets;
using QSP.LibraryExtension.XmlSerialization;
using QSP.RouteFinding.FileExport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;
using static QSP.RouteFinding.FileExport.Providers.Types;
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
        public IReadOnlyDictionary<SimulatorType, string> SimulatorPaths { get; private set; }

        // This option is only changed when 'export flight plan' button is clicked
        // in ExportMenu.
        public IReadOnlySet<ExportCommand> ExportCommands { get; private set; }

        public AppOptions(
            string NavDataLocation,
            bool PromptBeforeExit,
            bool AutoDLTracks,
            bool AutoDLWind,
            bool EnableWindOptimizedRoute,
            bool HideDctInRoute,
            bool ShowTrackIdOnly,
            bool AutoUpdate,
            IReadOnlyDictionary<SimulatorType, string> SimulatorPaths,
            IReadOnlySet<ExportCommand> ExportCommands)
        {
            this.NavDataLocation = NavDataLocation;
            this.PromptBeforeExit = PromptBeforeExit;
            this.AutoDLTracks = AutoDLTracks;
            this.AutoDLWind = AutoDLWind;
            this.EnableWindOptimizedRoute = EnableWindOptimizedRoute;
            this.HideDctInRoute = HideDctInRoute;
            this.ShowTrackIdOnly = ShowTrackIdOnly;
            this.AutoUpdate = AutoUpdate;
            this.SimulatorPaths = SimulatorPaths;
            this.ExportCommands = ExportCommands;
        }

        public static AppOptions Default => new AppOptions(
            Path.GetFullPath("../NavData"),
            true,
            true,
            true,
            true,
            false,
            false,
            true,
            Enums.GetValues<SimulatorType>().ToDictionary(t => t, t => ""),
            DefaultExportCommands());

        public class Serializer : IXSerializer<AppOptions>
        {
            public XElement Serialize(AppOptions a, string name)
            {
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

                    new XElement("SimulatorPaths", a.SimulatorPaths.Select(kv =>
                        new XElement("e", new XElement("type", (int)kv.Key),
                                          new XElement("path", kv.Value)))),

                    new XElement("ExportOptions", a.ExportCommands.Select(c => c.Serialize("e"))),
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

                    () =>
                    {
                        var dict = item.Element("SimulatorPaths")
                                       .Elements("e")
                                       .ToDictionary(e => (SimulatorType)e.GetInt("type"),
                                                     e => e.GetString("path"));

                        d.SimulatorPaths = d.SimulatorPaths.Update(dict);
                    },

                    () =>
                    {
                        var dict = item.Element("ExportOptions")
                                      .Elements("e")
                                      .Select(ExportCommand.Deserialize)
                                      .ToDictionary(x => x.ProviderType, x => x);

                        var old = d.ExportCommands.ToDictionary(x => x.ProviderType, x => x);

                        d.ExportCommands = old.Update(dict).Select(kv => kv.Value).ToReadOnlySet();
                    }
                };

                foreach (var a in actions)
                {
                    IgnoreException(a);
                }

                return d;
            }
        }

    }
}
