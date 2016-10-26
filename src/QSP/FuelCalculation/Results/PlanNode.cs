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
        public static readonly IReadOnlyList<Type> AllowedNodeTypes =
            new Type[] { typeof(RouteNode) };

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
            if (!IsValidType(NodeValue))
            {
                throw new ArgumentException("Type not allowed.");
            }

            this.NodeValue = NodeValue;
            this.TimeRemainingMin = TimeRemainingMin;
            this.AltitudeFt = AltitudeFt;
            this.TasKnots = TasKnots;
            this.FuelOnBoardTon = FuelOnBoardTon;
        }

        private static bool IsValidType(object NodeValue)
        {
            var type = NodeValue.GetType();
            return AllowedNodeTypes.Any(t => t.IsAssignableFrom(type));
        }
    }
}
