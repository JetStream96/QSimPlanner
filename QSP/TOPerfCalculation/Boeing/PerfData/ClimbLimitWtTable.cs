using QSP.MathTools.Tables;
using QSP.MathTools.Tables.ReadOnlyTables;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class ClimbLimitWtTable
    {
        private Table2D table;

        public ReadOnlyTable2D Table
        {
            get
            {
                return new ReadOnlyTable2D(table);
            }
        }

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
