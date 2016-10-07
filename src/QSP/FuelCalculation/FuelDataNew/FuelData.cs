using System.Xml.Linq;

namespace QSP.FuelCalculation.FuelDataNew
{
    public class FuelData
    {
        public FuelDataItem Data { get; private set; }
        public string ProfileName { get; private set; }
        public string FilePath { get; private set; }

        public FuelData(FuelDataItem Data, string ProfileName, string FilePath)
        {
            this.Data = Data;
            this.ProfileName = ProfileName;
            this.FilePath = FilePath;
        }

        public static FuelData FromFile(string path)
        {
            var root = XDocument.Load(path).Root;

            return new FuelData(
                new FuelDataItem.Serializer().Deserialize(root),
                root.Element("ProfileName").Value,
                path);
        }
    }
}
