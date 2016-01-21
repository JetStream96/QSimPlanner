using QSP.AviationTools;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Common.Parser;
using System;
using System.Linq;

namespace QSP.RouteFinding.Tracks.Ausots.Utilities
{
    // Parse the string which represents a single track, into an AusTrack.
    //
    // Sample input:
    // TDM TRK MY15 151112233001 
    // 1511122330 1511131400 
    // JAMOR IBABI LEC CALAR OOD 25S133E LEESA 20S128E 15S122E ATMAP 
    // RTS/YMML ML H164 JAMOR 
    // RMK/AUSOTS GROUP A
    //

    public class IndividualAusotsParser
    {
        private static readonly LatLon PreferredFirstLatLon = new LatLon(-25.0, 133.0);

        private string text;
        private AirportManager airportList;
        
        public IndividualAusotsParser(string text, AirportManager airportList)
        {
            this.text = text;
            this.airportList = airportList;
        }

        /// <summary>
        /// Returns null is the given string represents a track which is currently not available.
        /// </summary>
        /// <exception cref="TrackParseException"></exception>
        public AusTrack Parse()
        {
            var Result = new ParserTDMOld(text, airportList).Parse();

            if (TrackAvailble(Result))
            {
                return new AusTrack(Result.Ident,
                                    Result.TimeStart,
                                    Result.TimeEnd,
                                    Result.Remarks,
                                    Array.AsReadOnly(Result.MainRoute),
                                    Result.RouteFrom.AsReadOnly(),
                                    Result.RouteTo.AsReadOnly(),
                                    PreferredFirstLatLon);
            }
            return null;
        }

        // Sometimes, a TDM is like this (which indicates this track is not available):
        //
        // TDM TRK YS12 160119120001 
        // 1601191300 1601192200 
        // SVC TRK YS12 NO TRACK - USE PUBLISHED FIXED ROUTES
        // RMK/AUSOTS GROUP A USE KS12
        //
        private bool TrackAvailble(ParseResultOld Result )
        {
            return !(Result.MainRoute.Contains(Result.Ident) || Result.MainRoute.Contains("-"));
        }        
    }
}
