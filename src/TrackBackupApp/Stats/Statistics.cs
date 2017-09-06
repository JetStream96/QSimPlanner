using QSP.LibraryExtension.XmlSerialization;
using System.Xml.Linq;

namespace TrackBackupApp.Stats
{
    public class Statistics
    {
        public int WestboundDownloads { get; set; }
        public int EastboundDownloads { get; set; }
        public int UpdateChecks{ get; set; }

        public class Serializer : IXSerializer<Statistics>
        {
            private static readonly string WestboundText = "WestboundDownloads";
            private static readonly string EastboundText = "EastboundDownloads";
            private static readonly string UpdateChecksText = "UpdateChecks";

            // @Throws
            public Statistics Deserialize(XElement elem)
            {
                return new Statistics
                {
                    WestboundDownloads = elem.GetInt(WestboundText),
                    EastboundDownloads = elem.GetInt(EastboundText),
                    UpdateChecks=elem.GetInt(UpdateChecksText)
                };
            }

            // @NoThrow
            public XElement Serialize(Statistics item, string name)
            {
                return new XElement(name, new[]
                {
                    new XElement(WestboundText, item.WestboundDownloads.ToString()),
                    new XElement(EastboundText, item.EastboundDownloads.ToString()),
                    new XElement(UpdateChecksText, item.UpdateChecks.ToString())
                });
            }
        }
        
    }
}