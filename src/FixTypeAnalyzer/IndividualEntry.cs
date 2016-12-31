using System.Collections.Generic;
using QSP.AviationTools.Coordinates;

namespace FixTypeAnalyzer
{
    public class IndividualEntry
    {
        public string Icao { get; }
        public string Line { get; }
        public double? Distance { get; }

        public IndividualEntry(string Icao, string Line, double? Distance)
        {
            this.Icao = Icao;
            this.Line = Line;
            this.Distance = Distance;
        }
    }
}