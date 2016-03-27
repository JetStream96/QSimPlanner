using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class ClimbLimitWtTable : Table2D
    {
        public ClimbLimitWtTable(double[] altitudes, double[] oat, double[][] climbLimWt)
            : base(altitudes, oat, climbLimWt)
        { }

        public double ClimbLimitWeight(double altFt, double oat)
        {
            return ValueAt(altFt, oat);
        }
    }
}
