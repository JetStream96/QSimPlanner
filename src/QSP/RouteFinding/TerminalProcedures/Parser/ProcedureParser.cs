using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.TerminalProcedures.Parser
{
    public class ProcedureParser
    {
        private readonly SectionSplitter.Type type;

        public ProcedureParser(SectionSplitter.Type type)
        {
            this.type = type;
        }

        public IEnumerable<ParseResult> Parse(IEnumerable<string> allLines)
        {
            var sections = SectionSplitter.Split(allLines, type);
            return sections.Select(s => GetEntry(s));
        }

        // Convert to SidEntry. If failed, returns null.
        private static ParseResult GetEntry(SectionSplitter.SplitEntry entry)
        {
            var lines = entry.Lines;
            var firstLine = lines[0].Split(',');
            if (firstLine.Length < 3) return null;
            var rwyOrTransition = firstLine[2];
            var wpts = lines.Skip(1).Select(line => GetWpt(line)).Where(w => w != null);

            return new ParseResult()
            {
                Name = firstLine[1],
                RunwayOrTransition = rwyOrTransition,
                Type = GetEntryType.GetType(rwyOrTransition),
                Waypoints = wpts.ToList(),
                EndWithVector = !FixTypes.HasCorrds(lines.Last().Split(',')[0])
            };
        }

        // If failed, returns null.
        private static Waypoint GetWpt(string line)
        {
            var words = line.Split(',');
            if (words.Length < 4) return null;
            var ident = words[1];

            double lat, lon;
            if (FixTypes.HasCorrds(words[0]) &&
                double.TryParse(words[2], out lat) &&
                double.TryParse(words[3], out lon))
            {
                return new Waypoint(ident, lat, lon);
            }

            return null;
        }

        public class ParseResult
        {
            public string RunwayOrTransition;
            public string Name;
            public IReadOnlyList<Waypoint> Waypoints;
            public EntryType Type;
            public bool EndWithVector;
        }
    }
}