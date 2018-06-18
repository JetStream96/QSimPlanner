using QSP.RouteFinding.FileExport.Providers;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.RouteFinding.FileExport
{
    public class ExportCommand
    {
        public ProviderType ProviderType { get; private set; }
        public string Directory { get; private set; }
        public string Extension { get; private set; } 
        public bool Enabled { get; private set; }

        public ExportCommand(
            ProviderType ProviderType, 
            string Directory,
            bool Enabled)
        {
            this.ProviderType = ProviderType;
            this.Directory = Directory;
            this.Extension = Types.GetExtension(ProviderType);
            this.Enabled = Enabled;
        }

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
            return new ExportCommand(
                (ProviderType)item.GetInt("Type"),
                item.GetString("Path"),
                item.GetBool("Enabled"));
        }

        public bool Equals(ExportCommand other)
        {
            return ProviderType == other.ProviderType &&
                Directory == other.Directory &&
                Extension == other.Extension &&
                Enabled == other.Enabled;
        }
    }
}
