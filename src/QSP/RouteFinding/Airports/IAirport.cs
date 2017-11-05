using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.Airports
{
    public interface IAirport : ICoordinate, IEquatable<IAirport>
    {
        string Icao { get; }
        string Name { get; }
        int Elevation { get; }
        bool TransAvail { get; }
        int TransAlt { get; }
        int TransLvl { get; }
        int LongestRwyLengthFt { get; }
        IReadOnlyList<IRwyData> Rwys { get; }
    }
}
