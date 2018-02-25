using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Airbus.PerfData
{
    public class TableDataNode
    {
        public string Flaps { get; set; }
        public int IsaOffset { get; set; }
        public Table2D Table { get; set; }
    }
}