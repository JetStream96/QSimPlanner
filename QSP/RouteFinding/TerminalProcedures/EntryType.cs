using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QSP.RouteFinding.Utilities;

namespace QSP.RouteFinding.TerminalProcedures
{
    public enum EntryType
    {
        RwySpecific,
        Common,
        Transition
    }

    public static class GetEntryType
    {
        public static EntryType GetType(string rwy)
        {
            if (IsRwyIdent(rwy))
            {
                return EntryType.RwySpecific;
            }
            else if (rwy == "ALL")
            {
                return EntryType.Common;
            }
            else
            {
                return EntryType.Transition;
            }
        }
    }
}
