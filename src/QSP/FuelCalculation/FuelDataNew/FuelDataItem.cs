using QSP.LibraryExtension.XmlSerialization;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.FuelCalculation.FuelDataNew
{
    public class FuelDataItem
    {
        public double HoldingFuelPerMinuteKg { get; private set; }
        public double HoldingFuelRefWtTon { get; private set; }
        public double TaxiFuelPerMinKg { get; private set; }
        public double ApuFuelPerMinKg { get; private set; }
        public double MissedAppFuelKG { get; private set; }
        public double ClimbKias { get; private set; }
        public double DescendKias { get; private set; }
        public double CruiseMach { get; private set; }
        public DataPoint DataPoint1 { get; private set; }
        public DataPoint DataPoint2 { get; private set; }

        public FuelDataItem(
             double HoldingFuelPerMinuteKg,
             double HoldingFuelRefWtTon,
             double TaxiFuelPerMinKg,
             double ApuFuelPerMinKg,
             double MissedAppFuelKG,
             double ClimbKias,
             double DescendKias,
             double CruiseMach,
             DataPoint DataPoint1,
             DataPoint DataPoint2)
        {
            this.HoldingFuelPerMinuteKg = HoldingFuelPerMinuteKg;
            this.HoldingFuelRefWtTon = HoldingFuelRefWtTon;
            this.TaxiFuelPerMinKg = TaxiFuelPerMinKg;
            this.ApuFuelPerMinKg = ApuFuelPerMinKg;
            this.MissedAppFuelKG = MissedAppFuelKG;
            this.ClimbKias = ClimbKias;
            this.DescendKias = DescendKias;
            this.CruiseMach = CruiseMach;
            this.DataPoint1 = DataPoint1;
            this.DataPoint2 = DataPoint2;
        }
        
        public class Serializer : IXSerializer<FuelDataItem>
        {
            private static string HoldingFuelPerMinuteKg = 
                "HoldingFuelPerMinute";
            private static string HoldingFuelRefWtTon = "HoldingFuelRefWt";
            private static string TaxiFuelPerMinKg = "TaxiFuelPerMin";
            private static string ApuFuelPerMinKg = "ApuFuelPerMin";
            private static string MissedAppFuelKG = "MissedAppFuel";
            private static string ClimbKias = "ClimbIAS";
            private static string DescendKias = "DescentIAS";
            private static string CruiseMach = "CruiseMach";
            private static string DataPoint = "DataPoint";

            public FuelDataItem Deserialize(XElement elem)
            {
                var deserializer = new DataPoint.Serializer();
                var pts = elem.Elements(DataPoint);

                return new FuelDataItem(
                    elem.GetDouble(HoldingFuelPerMinuteKg),
                    elem.GetDouble(HoldingFuelRefWtTon),
                    elem.GetDouble(TaxiFuelPerMinKg),
                    elem.GetDouble(ApuFuelPerMinKg),
                    elem.GetDouble(MissedAppFuelKG),
                    elem.GetDouble(ClimbKias),
                    elem.GetDouble(DescendKias),
                    elem.GetDouble(CruiseMach),
                    deserializer.Deserialize(pts.ElementAt(0)),
                    deserializer.Deserialize(pts.ElementAt(1)));
            }
            
            public XElement Serialize(FuelDataItem item, string name)
            {
                var serializer = new DataPoint.Serializer();

                return new XElement(name,
                    new XElement(HoldingFuelPerMinuteKg, 
                        item.HoldingFuelPerMinuteKg),
                    new XElement(HoldingFuelRefWtTon, 
                        item.HoldingFuelRefWtTon),
                    new XElement(TaxiFuelPerMinKg, item.TaxiFuelPerMinKg),
                    new XElement(ApuFuelPerMinKg, item.ApuFuelPerMinKg),
                    new XElement(MissedAppFuelKG, item.MissedAppFuelKG),
                    new XElement(ClimbKias, item.ClimbKias),
                    new XElement(DescendKias, item.DescendKias),
                    new XElement(CruiseMach, item.CruiseMach),
                    serializer.Serialize(item.DataPoint1, DataPoint),
                    serializer.Serialize(item.DataPoint2, DataPoint));
            }
        }
    }
}
