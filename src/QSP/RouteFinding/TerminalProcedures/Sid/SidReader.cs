using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.TerminalProcedures.Parser;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // Read from file and gets a SidCollection for an airport.
    public static class SidReader
    {
        public static SidCollection Parse(IEnumerable<string> allLines)
        {
            var parseResult = new ProcedureParser(SectionSplitter.Type.Sid).Parse(allLines);
            return new SidCollection(parseResult.Select(r => Convert(r)));
        }

        private static SidEntry Convert(ProcedureParser.ParseResult r)
        {
            return new SidEntry(r.RunwayOrTransition, r.Name, r.Waypoints, r.Type, r.EndWithVector);
        }
    }
}
