using QSP.Common.Options;
using QSP.RouteFinding.FileExport.Providers;
using System;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.RouteFinding.FileExport
{
    public class ExportCommand : IEquatable<ExportCommand>
    {
        public ProviderType ProviderType { get; private set; }

        /// <summary>
        /// Can be empty if no custom directory is set.
        /// </summary>
        public string CustomDirectory { get; private set; }

        public bool Enabled { get; private set; }

        /// <summary>
        /// Can be null if default export path is a custom path.
        /// </summary>
        public SimulatorType? DefaultSimulator { get; private set; }


        public string Extension => Types.GetExtension(ProviderType);

        public ExportCommand(ProviderType ProviderType, string CustomDirectory,
            bool Enabled, SimulatorType? DefaultSimulator)
        {
            this.ProviderType = ProviderType;
            this.CustomDirectory = CustomDirectory;
            this.Enabled = Enabled;
            this.DefaultSimulator = DefaultSimulator;
        }

        public XElement Serialize(string name)
        {
            var elem = new XElement[]
            {
                ((int)ProviderType).Serialize("Type"),
                CustomDirectory.Serialize("Path"),
                Enabled.Serialize("Enabled"),
                (DefaultSimulator.HasValue ? ((int)DefaultSimulator):-1).Serialize("Sim")
            };

            return new XElement(name, elem);
        }

        public static ExportCommand Deserialize(XElement item)
        {
            SimulatorType? GetSim()
            {
                var sim = item.GetInt("Sim");
                return sim == -1 ? null : (SimulatorType?)sim;
            }

            return new ExportCommand((ProviderType)item.GetInt("Type"),
                item.GetString("Path"),
                item.GetBool("Enabled"),
                GetSim());
        }

        public override int GetHashCode()
        {
            return ProviderType.GetHashCode() ^
                CustomDirectory.GetHashCode() ^
                Enabled.GetHashCode() ^
                DefaultSimulator.GetHashCode();
        }

        public bool Equals(ExportCommand other)
        {
            return ProviderType == other.ProviderType &&
                CustomDirectory == other.CustomDirectory &&
                Enabled == other.Enabled &&
                DefaultSimulator == other.DefaultSimulator;
        }
    }
}
