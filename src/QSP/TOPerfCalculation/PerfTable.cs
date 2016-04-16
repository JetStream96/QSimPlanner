namespace QSP.TOPerfCalculation
{
    public class PerfTable
    {
        public PerfTableItem Item { get; private set; }
        public Entry Entry { get; private set; }

        public PerfTable(PerfTableItem Item, Entry Entry)
        {
            this.Item = Item;
            this.Entry = Entry;
        }
    }
}
