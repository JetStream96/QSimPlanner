using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class ClimbLimitWtTable
    {
        private Table2D table;

        public ClimbLimitWtTable(Table2D table)
        {
            this.table = table;
        }

        public double ClimbLimitWeight(double altFt, double oat)
        {
            return table.ValueAt(altFt, oat);
        }
    }
}
