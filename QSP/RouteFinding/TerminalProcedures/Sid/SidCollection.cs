using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // Collection of SidEntry for a particular airport.
    public class SidCollection
    {
        private List<SidEntry> _sids;

        public ReadOnlyCollection<SidEntry > SidList
        {
            get
            {
                return new ReadOnlyCollection<SidEntry>(_sids);
            }
        }

        public SidCollection(List<SidEntry>  sids)
        {
            _sids = sids;
        }
    }
}
