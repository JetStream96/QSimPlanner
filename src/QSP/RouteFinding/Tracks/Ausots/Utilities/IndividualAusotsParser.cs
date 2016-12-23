using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Common.TDM.Parser;
using System;
using System.Linq;
using QSP.RouteFinding.Routes;
using System.Collections.Generic;

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
        private static readonly LatLon PreferredLatLon = new LatLon(-25.0, 133.0);

        private readonly string text;
        private readonly AirportManager airportList;

        public IndividualAusotsParser(string text, AirportManager airportList)
        {
            this.text = text;
            this.airportList = airportList;
        }

        /// <summary>
        /// Returns null is the given string represents a track which is 
        /// currently not available.
        /// </summary>
        /// <exception cref="TrackParseException"></exception>
        public AusTrack Parse()
        {
            var result = new TdmParser(text).Parse();

            var mainRoute = result.MainRoute.Split(
                new char[] { ' ', '\t', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries)
                .ToRouteString();

            var connectRoutes = ConnectionRouteInterpreter.Convert(
                mainRoute,
                result.ConnectionRoutes,
                airportList);

            if (TrackAvailble(mainRoute, result.Ident))
            {
                return new AusTrack(
                    result.Ident,
                    result.TimeStart,
                    result.TimeEnd,
                    result.Remarks,
                    mainRoute,
                    connectRoutes.RouteFrom,
                    connectRoutes.RouteTo,
                    PreferredLatLon,
                    PreferredLatLon);
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
        private bool TrackAvailble(IEnumerable<string> mainRoute, string ident)
        {
            return !(mainRoute.Contains(ident) || mainRoute.Contains("NO TRACK"));
        }
    }
}
