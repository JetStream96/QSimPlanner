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

        // TODO: implement this.
        public class Serializer : IXSerializer<DataPoint>
        {
            public DataPoint Deserialize(XElement elem)
            {
                throw new NotImplementedException();
            }

            public XElement Serialize(DataPoint item, string name)
            {
                throw new NotImplementedException();
            }
        }
    }
}
