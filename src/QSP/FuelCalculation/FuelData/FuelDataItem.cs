using QSP.FuelCalculation.FuelData.Corrections.Boeing;
using QSP.LibraryExtension.XmlSerialization;
using System;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.FuelCalculation.FuelData
{
    // The units of variables used in this class is specified in 
    // FuelCalculation/Calculations/VariableUnitStandard.txt.

    public class FuelDataItem
    {
        public double HoldingFuelFlow { get; private set; }
        public double HoldingFuelRefWt { get; private set; }
        public double TaxiFuelFlow { get; private set; }
        public double ApuFuelFlow { get; private set; }
        public double ClimbKias { get; private set; }
        public double DescendKias { get; private set; }
        public DataPoint DataPoint1 { get; private set; }
        public DataPoint DataPoint2 { get; private set; }
        public FuelTable FuelTable { get; }

        public FuelDataItem(
             double HoldingFuelFlow,
             double HoldingFuelRefWt,
             double TaxiFuelFlow,
             double ApuFuelFlow,
             double ClimbKias,
             double DescendKias,
             DataPoint DataPoint1,
             DataPoint DataPoint2,
             FuelTable FuelTable = null)
        {
            this.HoldingFuelFlow = HoldingFuelFlow;
            this.HoldingFuelRefWt = HoldingFuelRefWt;
            this.TaxiFuelFlow = TaxiFuelFlow;
            this.ApuFuelFlow = ApuFuelFlow;
            this.ClimbKias = ClimbKias;
            this.DescendKias = DescendKias;
            this.DataPoint1 = DataPoint1;
            this.DataPoint2 = DataPoint2;
            this.FuelTable = FuelTable;
        }

        public class Serializer : IXSerializer<FuelDataItem>
        {
            private static string HoldingFuelPerMinuteKg = "HoldingFuelFlow";
            private static string HoldingFuelRefWt = "HoldingFuelRefWt";
            private static string TaxiFuelFlow = "TaxiFuelFlow";
            private static string ApuFuelFlow = "ApuFuelFlow";
            private static string ClimbKias = "ClimbIAS";
            private static string DescendKias = "DescentIAS";
            private static string DataPoint = "DataPoint";

            public FuelDataItem Deserialize(XElement elem)
            {
                var deserializer = new DataPoint.Serializer();
                var pts = elem.Elements(DataPoint).ToList();
                var table = elem.Element("FuelTable")?.GetString("Text");

                return new FuelDataItem(
                    elem.GetDouble(HoldingFuelPerMinuteKg),
                    elem.GetDouble(HoldingFuelRefWt),
                    elem.GetDouble(TaxiFuelFlow),
                    elem.GetDouble(ApuFuelFlow),
                    elem.GetDouble(ClimbKias),
                    elem.GetDouble(DescendKias),
                    deserializer.Deserialize(pts[0]),
                    deserializer.Deserialize(pts[1]),
                    table==null? null: new FuelTable(table));
            }

            public XElement Serialize(FuelDataItem item, string name)
            {
                // No need to implement this.
                throw new NotImplementedException();
            }
        }
    }
}
