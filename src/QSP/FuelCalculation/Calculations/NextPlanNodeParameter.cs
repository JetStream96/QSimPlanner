using System;
using System.Linq;
using QSP.FuelCalculation.Results.Nodes;
using QSP.RouteFinding.Routes;

namespace QSP.FuelCalculation.Calculations
{
    // See VariableUnitStandard.txt for the units of variables.
    public class NextPlanNodeParameter
    {
        public VerticalMode ModeVertical { get; }

        // The type can be either RouteNode or IntermediateNode. The other 
        // node types, i.e. TOC, SC, TOD, are marked after the PlanNode is
        // generated.
        public Type NodeType { get; }

        // Time to next PlanNode.
        public double StepTime { get; }

        public NextPlanNodeParameter(
            VerticalMode ModeVertical, Type NodeType, double StepTime)
        {
            // Validate type.
            if (NodeType != typeof(RouteNode) &&
                NodeType != typeof(IntermediateNode))
            {
                throw new ArgumentException();
            }

            this.ModeVertical = ModeVertical;
            this.NodeType = NodeType;
            this.StepTime = StepTime;
        }

        public enum VerticalMode
        {
            Climb,
            Cruise,
            Descent
        }
    }
}