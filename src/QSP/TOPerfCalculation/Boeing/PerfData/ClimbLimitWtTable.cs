using QSP.MathTools.TablesNew;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class ClimbLimitWtTable
    {
        private readonly Table table;

        public ClimbLimitWtTable(double[] altitudes, double[] oat, double[][] climbLimWt)
        {
            table = TableBuilder.Build2D(altitudes, oat, climbLimWt);
        }

        public double ClimbLimitWeight(double altFt, double oat)
        {
            return table.ValueAt(altFt, oat);
        }
    }
}
