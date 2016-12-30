using System.Collections.Generic;

namespace FixTypes
{
    public class Entry
    {
        public string Icao { get; }
        public IEnumerable<string> Lines { get; }

        public Entry(string Icao, IEnumerable<string> Lines)
        {
            this.Icao = Icao;
            this.Lines = Lines;
        }
    }

    public struct IndividualEntry
    {
        public string Icao { get; }
        public string Line { get; }

        public IndividualEntry(string Icao, string Line)
        {
            this.Icao = Icao;
            this.Line = Line;
        }
    }
}