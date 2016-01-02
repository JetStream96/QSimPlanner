using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.TerminalProcedures
{
    public interface IProcedureEntry
    {
        string RunwayOrTransition { get; }
        string Name { get; }
        EntryType Type { get; }
    }
}
