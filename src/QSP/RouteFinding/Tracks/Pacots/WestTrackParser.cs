using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Common.TDM.Parser;
using System;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.Tracks.Pacots
{
    // input example:
    //
    //(TDM TRK C 151105190001 
    //1511051900 1511060800 
    //MANJO 53N140W 56N150W 57N160W SPY CREMR 56N180E OPAKE OLCOT 
    //OPHET OGDEN OMOTO 
    //RTS/ CYVR V317 QQ MANJO 
    //KSEA TOU FINGS MANJO 
    //KPDX TOU FINGS MANJO 
    //KSFO TOU FINGS MANJO 
    //KLAX TOU FINGS MANJO 
    //OMOTO R580 OATIS 
    //RMK/ TRK ADVISORY IN EFFECT FOR TRK C 
    //). 05 NOV 19:00 2015 UNTIL 06 NOV 08:00 2015. CREATED: 05 NOV 02:15 2015
    //

    public class WestTrackParser
    {
        private AirportManager airportList;
        private string text;

        public WestTrackParser(string text, AirportManager airportList)
        {
            this.text = text;
            this.airportList = airportList;
        }

        public PacificTrack Parse()
        {
            var result = new TdmParser(text).Parse();

            var mainRoute = result.MainRoute.Split(
                new char[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries)
                .ToRouteString();

            var connectRoutes = ConnectionRouteInterpreter.Convert(
                mainRoute, 
                result.ConnectionRoutes,
                airportList);

            return new PacificTrack(
                PacotDirection.Westbound,
                result.Ident,
                result.TimeStart,
                result.TimeEnd,
                CutOffTextAfterParenthesis(result.Remarks),
                mainRoute,
                connectRoutes.RouteFrom,
                connectRoutes.RouteTo,
                Constants.JapanLatlon,
                Constants.USLatlon);
        }

        private static string CutOffTextAfterParenthesis(string item)
        {
            int index = item.IndexOf(')');
            if (index < 0)  return item;
            return item.Substring(0, index);
        }
    }
}
