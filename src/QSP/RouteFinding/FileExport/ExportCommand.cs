using QSP.RouteFinding.FileExport.Providers;
using System;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.RouteFinding.FileExport
{
    public class ExportCommand : IEquatable<ExportCommand>
    {
        public ProviderType ProviderType { get; set; }

        /// <summary>
        /// If this is empty, it means the default directory is used.
        /// </summary>
        public string Directory { get; set; }

        public bool Enabled { get; set; }

        public XElement Serialize(string name)
        {
            var elem = new XElement[]
            {
                ((int)ProviderType).Serialize("Type"),
                Directory.Serialize("Path"),
                Enabled.Serialize("Enabled")
            };

            return new XElement(name, elem);
        }

        public static ExportCommand Deserialize(XElement item)
        {
            return new ExportCommand()
            {
                ProviderType = (ProviderType)item.GetInt("Type"),
                Directory = item.GetString("Path"),
                Enabled = item.GetBool("Enabled")
            };
        }

        public bool Equals(ExportCommand other)
        {
            return ProviderType == other.ProviderType &&
                Directory == other.Directory &&
                Enabled == other.Enabled;
        }
    }
}
