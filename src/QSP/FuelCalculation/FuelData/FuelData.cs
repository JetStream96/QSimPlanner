using System.IO;
using System.Xml.Linq;

namespace QSP.FuelCalculation.FuelData
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
                LoadItem(path),
                root.Element("ProfileName").Value,
                path);
        }

        private static FuelDataItem LoadItem(string path)
        {
            var root = XDocument.Load(path).Root;
            var loc = root.Element("FileLocation");
            if (loc == null) return new FuelDataItem.Serializer().Deserialize(root);

            var biasAttribute = loc.Attribute("bias");
            var bias = biasAttribute == null ? 1.0 : double.Parse(biasAttribute.Value);
            return LoadItem(Path.Combine(path, "..", loc.Value)).WithBias(bias);
        }
    }
}
