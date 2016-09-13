using QSP.AviationTools;
using System;
using System.Xml.Linq;

namespace QSP.FuelCalculation.Tables
{
    public class SpeedProfile
    {
        private readonly double SpeedLimit10000Ft = 250.0;

        public double ClimbAirspeedKnots { get; private set; }
        public double DescendAirspeedKnots { get; private set; }
        public double CruizeMachNumber { get; private set; }

        public SpeedProfile(
            double ClimbAirspeedKnots,
            double DescendAirspeedKnots,
            double CruizeMachNumber)
        {
            this.ClimbAirspeedKnots = ClimbAirspeedKnots;
            this.DescendAirspeedKnots = DescendAirspeedKnots;
            this.CruizeMachNumber = CruizeMachNumber;
        }

        public static SpeedProfile FromXml(XElement node)
        {
            Func<string, double> parse = s =>
            double.Parse(node.Element(s).Value);

            return new SpeedProfile(
                parse("ClimbSpeed"),
                parse("DescendSpeed"),
                parse("CruiseSpeed"));
        }

        public double CruiseTasKnots(double altFT)
        {
            if (altFT <= 10000.0)
            {
                return SpeedLimit10000Ft;
            }

            return SpeedConversion.TasKnots(CruizeMachNumber, altFT);
        }
    }
}
