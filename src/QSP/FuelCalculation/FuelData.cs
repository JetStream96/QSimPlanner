using System.Xml.Linq;

namespace QSP.FuelCalculation
{/*
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
                FuelDataItem.FromFile(path),
                root.Element("General").Element("ProfileName").Value,
                path);
        }
    }*/
}
