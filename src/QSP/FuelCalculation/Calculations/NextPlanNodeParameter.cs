using System;

namespace QSP.FuelCalculation.Calculations
{
    public class NextPlanNodeParameter
    {
        public VerticalMode ModeVertical { get; }
        public Type NodeType { get; }

        public NextPlanNodeParameter(VerticalMode ModeVertical, Type NodeType)
        {
            this.ModeVertical = ModeVertical;
            this.NodeType = NodeType;
        }

        public enum VerticalMode
        {
            Climb,
            Cruise,
            Descent
        }
    }
}