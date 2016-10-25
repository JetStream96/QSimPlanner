using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.FuelCalculation.Results
{
    public class PlanNode
    {
        public object NodeValue { get; private set; }
        public double TimeRemainingMin { get; private set; }
        public double AltitudeFt { get; private set; }
        public double TasKnots { get; private set; }
        public double FuelOnBoardTon { get; private set; }

        public PlanNode(
            object NodeValue,
            double TimeRemainingMin,
            double AltitudeFt,
            double TasKnots,
            double FuelOnBoardTon)
        {
            this.NodeValue = NodeValue;
            this.TimeRemainingMin = TimeRemainingMin;
            this.AltitudeFt = AltitudeFt;
            this.TasKnots = TasKnots;
            this.FuelOnBoardTon = FuelOnBoardTon;
        }        
    }
}
