using QSP.MathTools.Tables;

namespace QSP.TOPerfCalculation.Airbus.DataClasses
{
    public class TableDataNode
    {
        public string Flaps { get; set; }
        public double IsaOffset { get; set; }
        public Table2D Table { get; set; }

        public bool Equals(TableDataNode other, double delta)
        {
            return other != null &&
                Flaps == other.Flaps &&
                Table.Equals(other.Table, delta);
        }
    }
}