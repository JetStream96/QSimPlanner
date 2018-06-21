using QSP.Common.Options;
using QSP.RouteFinding.FileExport.Providers;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.RouteFinding.FileExport
{
    public class ExportCommand
    {
        public ProviderType ProviderType { get; set; }

        /// <summary>
        /// Can be empty if no custom directory is set.
        /// </summary>
        public string CustomDirectory { get; set; }

        public bool Enabled { get; set; }

        /// <summary>
        /// Can be null if default export path is a custom path.
        /// </summary>
        public SimulatorType? DefaultSimulator { get; set; }


        public string Extension => Types.GetExtension(ProviderType);

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

            return new ExportCommand()
            {

                ProviderType = (ProviderType)item.GetInt("Type"),
                CustomDirectory = item.GetString("Path"),
                Enabled = item.GetBool("Enabled"),
                DefaultSimulator = GetSim()
            };
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
