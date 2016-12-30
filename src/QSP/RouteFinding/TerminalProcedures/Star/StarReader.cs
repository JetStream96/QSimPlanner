using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.TerminalProcedures.Parser;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    // Read from file and gets a StarCollection for an airport.
    //
    public static class StarReader
    {
        public static StarCollection Parse(IEnumerable<string> allLines)
        {
            var parseResult = new ProcedureParser(SectionSplitter.Type.Star).Parse(allLines);
            return new StarCollection(parseResult.Select(r => Convert(r)));
        }

        private static StarEntry Convert(ProcedureParser.ParseResult r)
        {
            return new StarEntry(r.RunwayOrTransition, r.Name, r.Waypoints, r.Type);
        }
    }
}

