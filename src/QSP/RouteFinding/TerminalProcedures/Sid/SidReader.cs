using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.TerminalProcedures.Parser;
using static QSP.RouteFinding.FixTypes;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // Read from file and gets a SidCollection for an airport.
    public class SidReader
    {
        private IEnumerable<string> allLines;

        public SidReader() { }

        /// <exception cref="ArgumentNullException"></exception>
        public SidReader(string allText) : this(allText.Lines()) { }

        /// <exception cref="ArgumentNullException"></exception>
        public SidReader(IEnumerable<string> allLines)
        {
            if (allLines == null) throw new ArgumentNullException();
            this.allLines = allLines;
        }

        public SidCollection Parse()
        {
            var sections = SectionSplitter.Split(allLines, SectionSplitter.Type.Sid);
            var sids = new List<SidEntry>();

            bool isInSidBody = false;
            string name = null;
            string rwyOrTransition = null;
            bool endWithVector = false;
            List<Waypoint> wpts = null;

            foreach (var line in allLines)
            {
                var words = line.Split(',');

                if (words[0] == "SID")
                {
                    isInSidBody = true;

                    // Add last SID if exists.
                    AddLastEntry(sids, name, rwyOrTransition, endWithVector, wpts);

                    // This is a new SID.
                    name = words[1];
                    rwyOrTransition = words[2];
                    wpts = new List<Waypoint>();
                }
                else if (isInSidBody)
                {
                    if (IsEmptyLine(line))
                    {
                        isInSidBody = false;
                    }
                    else
                    {
                        // This is a waypoint (or vector, etc) in SID.
                        if (HasCorrds(words[0]))
                        {
                            endWithVector = false;
                            wpts.Add(GetWpt(words));
                        }
                        else
                        {
                            endWithVector = true;
                        }
                    }
                }
            }

            // Add the last SID.
            AddLastEntry(sids, name, rwyOrTransition, endWithVector, wpts);

            return new SidCollection(sids);
        }

        // Convert to SidEntry. If failed, returns null.
        private static SidEntry GetEntry(SectionSplitter.SplitEntry entry)
        {
            var firstLine = entry.Lines[0].Split(',');
            if (firstLine.Length < 3) return null;

        }

        private static void AddLastEntry(
            List<SidEntry> sids,
            string name,
            string rwyOrTransition,
            bool endWithVector,
            List<Waypoint> wpts)
        {
            if (name != null)
            {
                var entry = new SidEntry(
                    rwyOrTransition,
                    name,
                    wpts,
                    GetEntryType.GetType(rwyOrTransition),
                    endWithVector);

                sids.Add(entry);
            }
        }

        public static Waypoint GetWpt(string[] line)
        {
            var ident = line[1];
            double lat = double.Parse(line[2]);
            double lon = double.Parse(line[3]);

            return new Waypoint(ident, lat, lon);
        }

        public static bool IsEmptyLine(string line)
        {
            return !line.Contains(',');
        }
    }
}
