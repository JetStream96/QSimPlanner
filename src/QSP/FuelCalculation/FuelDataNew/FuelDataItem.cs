using QSP.FuelCalculation.Tables;
using QSP.LibraryExtension.XmlSerialization;
using QSP.Utilities.Units;
using System;
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
        
        // TODO: implement this.
        public class Serializer : IXSerializer<FuelDataItem>
        {
            public FuelDataItem Deserialize(XElement elem)
            {
                throw new NotImplementedException();
            }

            public XElement Serialize(FuelDataItem item, string name)
            {
                throw new NotImplementedException();
            }
        }
    }
}
