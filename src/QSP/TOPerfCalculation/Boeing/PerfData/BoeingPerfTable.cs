using System.Collections.Generic;
using System.Linq;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class BoeingPerfTable : PerfTableItem
    {
        public IReadOnlyList<IndividualPerfTable> Tables { get; }
        public IReadOnlyList<string> Flaps { get; }

        public BoeingPerfTable(IReadOnlyList<IndividualPerfTable> Tables)
        {
            this.Tables = Tables;
            Flaps = Tables.Select(x => x.Flaps).ToList();
        }
    }
}
