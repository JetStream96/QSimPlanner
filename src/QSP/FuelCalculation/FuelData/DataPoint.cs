using System.Xml.Linq;
using QSP.LibraryExtension.XmlSerialization;

namespace QSP.FuelCalculation.FuelData
{
    // The units of variables used in this class is specified in 
    // FuelCalculation/Calculations/VariableUnitStandard.txt.

    public class DataPoint
    {
        public double Weight { get; private set; }
        public double CruiseFuelFlow { get; private set; }
        public double CruiseKias { get; private set; }
        public double ClimbGradient { get; private set; }
        public double ClimbFuelFlow { get; private set; }
        public double DescentGradient { get; private set; }
        public double DescentFuelFlow { get; private set; }
        public double OptCruiseAlt { get; private set; }
        public double EtopsCruiseKtas { get; private set; }
        public double EtopsCruiseFuelFlow { get; private set; }

        public DataPoint(
            double Weight,
            double CruiseFuelFlow,
            double CruiseKias,
            double ClimbGradient,
            double ClimbFuelFlow,
            double DescentGradient,
            double DescentFuelFlow,
            double OptCruiseAlt,
            double EtopsCruiseKtas,
            double EtopsCruiseFuelFlow)
        {
            this.Weight = Weight;
            this.CruiseFuelFlow = CruiseFuelFlow;
            this.CruiseKias = CruiseKias;
            this.ClimbGradient = ClimbGradient;
            this.ClimbFuelFlow = ClimbFuelFlow;
            this.DescentGradient = DescentGradient;
            this.DescentFuelFlow = DescentFuelFlow;
            this.OptCruiseAlt = OptCruiseAlt;
            this.EtopsCruiseKtas = EtopsCruiseKtas;
            this.EtopsCruiseFuelFlow = EtopsCruiseFuelFlow;
        }

        public class Serializer : IXSerializer<DataPoint>
        {
            private static string Weight = "Weight";
            private static string CruiseFuelFlow = "CruiseFF";
            private static string CruiseKias = "CruiseIAS";
            private static string ClimbGradient = "ClimbGrad";
            private static string ClimbFuelFlow = "ClimbFF";
            private static string DescentGradient = "DescentGrad";
            private static string DescentFuelFlow = "DescentFF";
            private static string OptCruiseAlt = "OptCruiseAlt";
            private static string EtopsCruiseKtas = "EtopsCruiseTAS";
            private static string EtopsCruiseFuelFlow = "EtopsCruiseFF";

            public DataPoint Deserialize(XElement elem)
            {
                return new DataPoint(
                    elem.GetDouble(Weight),
                    elem.GetDouble(CruiseFuelFlow),
                    elem.GetDouble(CruiseKias),
                    elem.GetDouble(ClimbGradient),
                    elem.GetDouble(ClimbFuelFlow),
                    elem.GetDouble(DescentGradient),
                    elem.GetDouble(DescentFuelFlow),
                    elem.GetDouble(OptCruiseAlt),
                    elem.GetDouble(EtopsCruiseKtas),
                    elem.GetDouble(EtopsCruiseFuelFlow));
            }

            public XElement Serialize(DataPoint item, string name)
            {
                return new XElement(name,
                new XElement(Weight, item.Weight),
                new XElement(Weight, item.Weight),
                new XElement(CruiseFuelFlow, item.CruiseFuelFlow),
                new XElement(CruiseKias, item.CruiseKias),
                new XElement(ClimbGradient, item.ClimbGradient),
                new XElement(ClimbFuelFlow, item.ClimbFuelFlow),
                new XElement(DescentGradient, item.DescentGradient),
                new XElement(DescentFuelFlow, item.DescentFuelFlow),
                new XElement(OptCruiseAlt, item.OptCruiseAlt),
                new XElement(EtopsCruiseKtas, item.EtopsCruiseKtas),
                new XElement(EtopsCruiseFuelFlow,
                item.EtopsCruiseFuelFlow));
            }
        }
    }
}
