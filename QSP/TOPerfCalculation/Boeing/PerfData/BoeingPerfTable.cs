using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class BoeingPerfTable
    {
        private IndividualPerfTable[] tables;

        public string[] Flaps { get; private set; }

        public BoeingPerfTable(IndividualPerfTable[] tables)
        {
            this.tables = tables;
            Flaps = tables.Select(x => x.Flaps).ToArray();
        }

        public IndividualPerfTable GetTable(int flapsIndex)
        {
            return tables[flapsIndex];
        }
    }
}
