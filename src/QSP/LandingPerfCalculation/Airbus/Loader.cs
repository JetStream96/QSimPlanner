using QSP.LibraryExtension.XmlSerialization;
using System;
using System.Linq;
using System.Xml.Linq;

namespace QSP.LandingPerfCalculation.Airbus
{
    public static class Loader
    {
        /// <summary>
        /// Load an aircraft data from specified Xml file.
        /// </summary>
        /// /// <exception cref="Exception"></exception>
        public static PerfTable ReadFromXml(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var table = LoadPerfTable(doc.Root);
            return new PerfTable(table, LdgTableLoader.GetEntry(filePath, doc));
        }

        /// <summary>
        /// Loads the performance table when given the root node of xml file.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static AirbusPerfTable LoadPerfTable(XElement rootNode)
        {
            if (rootNode.Element("Parameters").Element("Format").Value != "1")
            {
                throw new Exception("Wrong format.");
            }

            return new AirbusPerfTable()
            {
                Entries = rootNode.Elements("Table").Select(e =>
                {
                    var (dry, wet) = TOPerfCalculation.Airbus.Loader.GetTable3Rows(e.Value);
                    return new Entry()
                    {
                        Dry = dry,
                        Wet = wet,
                        Flaps = e.GetAttributeString("flaps"),
                        Autobrake = e.GetAttributeString("autobrake"),
                        ElevationDry = e.GetAttributeDouble("elevation_dry"),
                        ElevationWet = e.GetAttributeDouble("elevation_wet"),
                        HeadwindDry = e.GetAttributeDouble("headwind_dry"),
                        HeadwindWet = e.GetAttributeDouble("headwind_wet"),
                        TailwindDry = e.GetAttributeDouble("tailwind_dry"),
                        TailwindWet = e.GetAttributeDouble("tailwind_wet"),
                        BothReversersDry = e.GetAttributeDouble("both_reverser_dry"),
                        BothReversersWet = e.GetAttributeDouble("both_reverser_wet"),
                        Speed5Knots = e.GetAttributeDouble("speed_5kts")
                    };
                }).ToList()
            };
        }
    }
}
