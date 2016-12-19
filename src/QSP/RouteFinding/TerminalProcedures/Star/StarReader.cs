using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using static QSP.RouteFinding.FixTypes;
using static QSP.RouteFinding.TerminalProcedures.Sid.SidReader;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    // Read from file and gets a StarCollection for an airport.
    //
    public class StarReader
    {
        private IEnumerable<string> allLines;

        public StarReader() { }

        /// <exception cref="ArgumentNullException"></exception>
        public StarReader(string allText) : this(allText.Lines()) { }

        /// <exception cref="ArgumentNullException"></exception>
        public StarReader(IEnumerable<string> allLines)
        {
            if (allLines == null)
            {
                throw new ArgumentNullException();
            }

            this.allLines = allLines;
        }

        public StarCollection Parse()
        {
            var stars = new List<StarEntry>();

            bool isInStarBody = false;
            string name = null;
            string rwyOrTransition = null;
            List<Waypoint> wpts = null;

            foreach (var line in allLines)
            {
                var words = line.Split(',');

                if (words[0] == "STAR")
                {
                    isInStarBody = true;

                    // Add last STAR if exists.
                    AddLastEntry(stars, name, rwyOrTransition, wpts);

                    // This is a new STAR.
                    name = words[1];
                    rwyOrTransition = words[2];
                    wpts = new List<Waypoint>();
                }
                else if (isInStarBody)
                {
                    if (IsEmptyLine(line))
                    {
                        isInStarBody = false;
                    }
                    else
                    {
                        // This is a waypoint (or vector, etc) in STAR.
                        if (HasCorrds(words[0]))
                        {
                            wpts.Add(GetWpt(words));
                        }
                    }
                }
            }

            // Add last STAR.
            AddLastEntry(stars, name, rwyOrTransition, wpts);

            return new StarCollection(stars);
        }

        private static void AddLastEntry(
            List<StarEntry> stars,
            string name,
            string rwyOrTransition,
            List<Waypoint> wpts)
        {
            if (name != null)
            {
                var entry = new StarEntry(
                    rwyOrTransition,
                    name,
                    wpts,
                    GetEntryType.GetType(rwyOrTransition));

                stars.Add(entry);
            }
        }
    }
}

