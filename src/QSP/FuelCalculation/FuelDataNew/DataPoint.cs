using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using QSP.LibraryExtension.XmlSerialization;

namespace QSP.FuelCalculation.FuelDataNew
{
    public class DataPoint
    {
        public double WeightKg { get; private set; }
        public double CruiseFuelPerMinKg { get; private set; }
        public double CruiseKias { get; private set; }
        public double ClimbGradient { get; private set; }
        public double ClimbFuelPerMinKg { get; private set; }
        public double DescentGradient { get; private set; }
        public double DescentFuelPerMinKg { get; private set; }
        public double OptCruiseAltFt { get; private set; }
        public double EtopsCruiseKtas { get; private set; }
        public double EtopsCruiseFuelPerMinKg { get; private set; }

        public DataPoint(
            double WeightKg,
            double CruiseFuelPerMinKg,
            double CruiseKias,
            double ClimbGradient,
            double ClimbFuelPerMinKg,
            double DescentGradient,
            double DescentFuelPerMinKg,
            double OptCruiseAltFt,
            double EtopsCruiseKtas,
            double EtopsCruiseFuelPerMinKg)
        {
            this.WeightKg = WeightKg;
            this.CruiseFuelPerMinKg = CruiseFuelPerMinKg;
            this.CruiseKias = CruiseKias;
            this.ClimbGradient = ClimbGradient;
            this.ClimbFuelPerMinKg = ClimbFuelPerMinKg;
            this.DescentGradient = DescentGradient;
            this.DescentFuelPerMinKg = DescentFuelPerMinKg;
            this.OptCruiseAltFt = OptCruiseAltFt;
            this.EtopsCruiseKtas = EtopsCruiseKtas;
            this.EtopsCruiseFuelPerMinKg = EtopsCruiseFuelPerMinKg;
        }
        
        public class Serializer : IXSerializer<DataPoint>
        {
            private static string WeightKg = "Weight";
            private static string CruiseFuelPerMinKg = "CruiseFF";
            private static string CruiseKias = "CruiseIAS";
            private static string ClimbGradient = "ClimbGrad";
            private static string ClimbFuelPerMinKg = "ClimbFF";
            private static string DescentGradient = "DescentGrad";
            private static string DescentFuelPerMinKg = "DescentFF";
            private static string OptCruiseAltFt = "OptCruiseAlt";
            private static string EtopsCruiseKtas = "EtopsCruiseTAS";
            private static string EtopsCruiseFuelPerMinKg = "EtopsCruiseFF";

            public DataPoint Deserialize(XElement elem)
            {
                return new DataPoint(
                    elem.GetDouble(WeightKg),
                    elem.GetDouble(CruiseFuelPerMinKg),
                    elem.GetDouble(CruiseKias),
                    elem.GetDouble(ClimbGradient),
                    elem.GetDouble(ClimbFuelPerMinKg),
                    elem.GetDouble(DescentGradient),
                    elem.GetDouble(DescentFuelPerMinKg),
                    elem.GetDouble(OptCruiseAltFt),
                    elem.GetDouble(EtopsCruiseKtas),
                    elem.GetDouble(EtopsCruiseFuelPerMinKg));
            }

            public XElement Serialize(DataPoint item, string name)
            {
                return new XElement(name,
                new XElement(WeightKg, item.WeightKg),
                new XElement(WeightKg, item.WeightKg),
                new XElement(CruiseFuelPerMinKg, item.CruiseFuelPerMinKg),
                new XElement(CruiseKias, item.CruiseKias),
                new XElement(ClimbGradient, item.ClimbGradient),
                new XElement(ClimbFuelPerMinKg, item.ClimbFuelPerMinKg),
                new XElement(DescentGradient, item.DescentGradient),
                new XElement(DescentFuelPerMinKg, item.DescentFuelPerMinKg),
                new XElement(OptCruiseAltFt, item.OptCruiseAltFt),
                new XElement(EtopsCruiseKtas, item.EtopsCruiseKtas),
                new XElement(EtopsCruiseFuelPerMinKg, 
                item.EtopsCruiseFuelPerMinKg));
            }
        }
    }
}
