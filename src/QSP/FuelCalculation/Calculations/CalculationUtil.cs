using System.Collections.Generic;
using static QSP.AviationTools.Heading.HeadingCalculation;
using static QSP.FuelCalculation.Calculations.NodeMarker;

namespace QSP.FuelCalculation.Calculations
{
    public static class CalculationUtil
    {
        public static (bool IsValid, double NewAlt) 
            CruiseAltValid(ICrzAltProvider altProvider, IReadOnlyList<IPlanNode> nodes)
        {
            int tocIndex = TocIndex(nodes);
            var toc = nodes[tocIndex];
            var heading = Heading(toc, nodes[tocIndex + 1]);
            bool valid = altProvider.IsValidCrzAlt(toc, heading, toc.Alt);
            double newAlt = valid ? toc.Alt :
                altProvider.ClosestAltBelow(toc, heading, toc.Alt);
            return (valid, newAlt);
        }
    }
}