using System.Collections.Generic;
using System.Linq;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class BoeingPerfTable : PerfTableItem
    {
        // The index of the flaps corresponds to the tables.
        public IReadOnlyList<IndividualPerfTable> Tables { get; }
        public IReadOnlyList<string> Flaps { get; }

        public BoeingPerfTable(IReadOnlyList<IndividualPerfTable> Tables)
        {
            this.Tables = Tables;
            Flaps = Tables.Select(x => x.Flaps).ToList();
        }
    }
}
