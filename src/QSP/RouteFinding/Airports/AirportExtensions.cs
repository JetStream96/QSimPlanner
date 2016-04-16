using System;

namespace QSP.RouteFinding.Airports
{
    public static class AirportExtensions
    {
        public static int RwyElevationFt(this Airport airport, string ident)
        {
            for (int j = 0; j < airport.Rwys.Count; j++)
            {
                if (airport.Rwys[j].RwyIdent == ident)
                {
                    return airport.Rwys[j].Elevation;
                }
            }

            throw new ArgumentException("Runway Not found.");
        }        
    }
}
