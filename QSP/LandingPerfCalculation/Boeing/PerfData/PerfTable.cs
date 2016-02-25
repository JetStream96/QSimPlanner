using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LandingPerfCalculation.Boeing.PerfData
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
