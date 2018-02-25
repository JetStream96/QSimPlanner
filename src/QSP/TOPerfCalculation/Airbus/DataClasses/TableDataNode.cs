using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Airbus.PerfData
{
    public class TableDataNode
    {
        public string Flaps { get; set; }
        public double IsaOffset { get; set; }
        public Table2D Table { get; set; }
    }
}