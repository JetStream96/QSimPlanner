using QSP.MathTools.Vectors;

namespace QSP.FuelCalculation.Calculations
{
    public class PlanNodeState
    {
        // Variable units:
        // Altitude: ft
        // Time: min
        // Distance: nm
        // Speed: knots
        // Climb/Descent rate: ft/min
        // Weight: kg
        // Fuel amount: kg
        // Fuel flow: kg/min

        public Vector3D V { get; }
        public double Alt { get; }
        public double GrossWt { get; }
        public double FuelOnBoard { get; }
        public double TimeRemaining { get; }
        public double Kias { get; }
        public double Ktas { get; }
        public double Gs { get; }


    }
}