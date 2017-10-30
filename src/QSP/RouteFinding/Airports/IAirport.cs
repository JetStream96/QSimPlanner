using System.Collections.Generic;

namespace QSP.RouteFinding.Airports
{
    public interface IAirport
    {
        string Icao { get; }
        string Name { get; }
        double Lat { get; }
        double Lon { get; }
        int Elevation { get; }
        bool TransAvail { get; }
        int TransAlt { get; }
        int TransLvl { get; }
        int LongestRwyLength { get; }
        IReadOnlyList<IRwyData> Rwys { get; }
    }
}
