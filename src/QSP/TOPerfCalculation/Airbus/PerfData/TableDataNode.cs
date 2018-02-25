using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Airbus.PerfData
{
    public class TableDataNode
    {
        public string Flaps { get; private set; }
        public int IsaOffset { get; private set; }
        public Table2D Table { get; set; }
    }
}