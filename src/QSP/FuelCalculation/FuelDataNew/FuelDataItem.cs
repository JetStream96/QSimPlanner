using QSP.LibraryExtension.XmlSerialization;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.FuelCalculation.FuelDataNew
{
    // The units of variables used in this class is specified in 
    // FuelCalculation/Calculations/VariableUnitStandard.txt.

    public class FuelDataItem
    {
        public double HoldingFuelFlow { get; private set; }
        public double HoldingFuelRefWt { get; private set; }
        public double TaxiFuelFlow { get; private set; }
        public double ApuFuelFlow { get; private set; }

        // Fuel for 1 missed approach
        public double MissedAppFuel { get; private set; }

        public double ClimbKias { get; private set; }
        public double DescendKias { get; private set; }
        public DataPoint DataPoint1 { get; private set; }
        public DataPoint DataPoint2 { get; private set; }

        public FuelDataItem(
             double HoldingFuelFlow,
             double HoldingFuelRefWt,
             double TaxiFuelFlow,
             double ApuFuelFlow,
             double MissedAppFuel,
             double ClimbKias,
             double DescendKias,
             DataPoint DataPoint1,
             DataPoint DataPoint2)
        {
            this.HoldingFuelFlow = HoldingFuelFlow;
            this.HoldingFuelRefWt = HoldingFuelRefWt;
            this.TaxiFuelFlow = TaxiFuelFlow;
            this.ApuFuelFlow = ApuFuelFlow;
            this.MissedAppFuel = MissedAppFuel;
            this.ClimbKias = ClimbKias;
            this.DescendKias = DescendKias;
            this.DataPoint1 = DataPoint1;
            this.DataPoint2 = DataPoint2;
        }

        public class Serializer : IXSerializer<FuelDataItem>
        {
            private static string HoldingFuelPerMinuteKg =
                "HoldingFuelFlow";
            private static string HoldingFuelRefWt = "HoldingFuelRefWt";
            private static string TaxiFuelFlow = "TaxiFuelFlow";
            private static string ApuFuelFlow = "ApuFuelFlow";
            private static string MissedAppFuel = "MissedAppFuel";
            private static string ClimbKias = "ClimbIAS";
            private static string DescendKias = "DescentIAS";
            private static string DataPoint = "DataPoint";

            public FuelDataItem Deserialize(XElement elem)
            {
                var deserializer = new DataPoint.Serializer();
                var pts = elem.Elements(DataPoint).ToList();

                return new FuelDataItem(
                    elem.GetDouble(HoldingFuelPerMinuteKg),
                    elem.GetDouble(HoldingFuelRefWt),
                    elem.GetDouble(TaxiFuelFlow),
                    elem.GetDouble(ApuFuelFlow),
                    elem.GetDouble(MissedAppFuel),
                    elem.GetDouble(ClimbKias),
                    elem.GetDouble(DescendKias),
                    deserializer.Deserialize(pts[0]),
                    deserializer.Deserialize(pts[1]));
            }

            public XElement Serialize(FuelDataItem item, string name)
            {
                var serializer = new DataPoint.Serializer();

                return new XElement(name,
                    new XElement(HoldingFuelPerMinuteKg,
                        item.HoldingFuelFlow),
                    new XElement(HoldingFuelRefWt,
                        item.HoldingFuelRefWt),
                    new XElement(TaxiFuelFlow, item.TaxiFuelFlow),
                    new XElement(ApuFuelFlow, item.ApuFuelFlow),
                    new XElement(MissedAppFuel, item.MissedAppFuel),
                    new XElement(ClimbKias, item.ClimbKias),
                    new XElement(DescendKias, item.DescendKias),
                    serializer.Serialize(item.DataPoint1, DataPoint),
                    serializer.Serialize(item.DataPoint2, DataPoint));
            }
        }
    }
}
