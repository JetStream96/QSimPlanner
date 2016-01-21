using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;
using static QSP.LibraryExtension.Arrays;
using QSP.RouteFinding.Tracks.Common;
using static QSP.LibraryExtension.StringParser.Utilities;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class WestTrackParser
    {
        //UPR means upper (airway). This should be ignored when parsing routeFrom/To.
        private static string[] SPECIAL_WORD = { "UPR" };

        private AirportManager airportList;
        private string text;

        private string[] mainRoute;
        private List<string[]> routeFrom;
        private List<string[]> routeTo;
        private string Ident;
        private string TimeStart;
        private string TimeEnd;
        private string Remarks;

        public WestTrackParser(string text, AirportManager airportList)
        {
            routeFrom = new List<string[]>();
            routeTo = new List<string[]>();
            this.text = text;
            this.airportList = airportList;
        }

        public PacificTrack Parse()
        {
            parseWest();
            removeRedundentFromList();
            convertAllLatLonFormat();

            return new PacificTrack(PacotDirection.Westbound,
                                    Ident,
                                    TimeStart,
                                    TimeEnd,
                                    Remarks,
                                    Array.AsReadOnly(mainRoute),
                                    routeFrom.AsReadOnly(),
                                    routeTo.AsReadOnly(),
                                    Constants.US_LATLON);
        }

        private void convertAllLatLonFormat()
        {
            Common.Utilities.ConvertLatLonFormat(mainRoute);

            foreach (var i in routeFrom)
            {
                Common.Utilities.ConvertLatLonFormat(i);
            }

            foreach (var j in routeTo)
            {
                Common.Utilities.ConvertLatLonFormat(j);
            }
        }

        private void removeRedundentFromList()
        {
            //choose distinct items
            routeFrom = Common.Utilities.SelectDistinct(routeFrom);
            routeTo = Common.Utilities.SelectDistinct(routeTo);

            //remove routes containing only 1 waypoint
            routeFrom.RemoveTinyArray(2);
            routeTo.RemoveTinyArray(2);
        }

        private void parseWest()
        {
            //input example:
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

            //get ident
            int index = text.IndexOf("TDM TRK") + "TDM TRK".Length;
            SkipAny(text, DelimiterWords, ref index);
            Ident = ReadToNextDelimeter(text, DelimiterWords, ref index);

            //get time start/end
            SkipToNextLine(text, ref index);
            SkipAny(text, DelimiterWords, ref index);
            TimeStart = ReadToNextDelimeter(text, DelimiterWords, ref index);
            SkipAny(text, DelimiterWords, ref index);
            TimeEnd = ReadToNextDelimeter(text, DelimiterWords, ref index);

            //get main route
            //TODO: what if RTS or RMK does not exist?
            y = text.IndexOf("RTS/", x);
            mainRoute = text.StringBetween(x, y).Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);

            //get route from/to
            y += "RTS/".Length - 1;

            int z = text.IndexOf("RMK/");

            while (y < z)
            {
                x = y;
                y = text.IndexOfAny(DelimiterLines, x + 1);

                if (y < z)
                {
                    addTrack(text.StringBetween(x, y));
                }
            }

            //get remarks
            Remarks = text.Substring(z + "RMK/".Length);
        }

        private void addTrack(string line)
        {
            var words = line.Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {
                if (words.Last() == mainRoute[0])
                {
                    if (airportList.Find(words[0]) != null)
                    {
                        routeFrom.Add(words.SubArray(1, words.Length - 1, SPECIAL_WORD));
                        return;
                    }
                    else
                    {
                        routeFrom.Add(words.Exclude(SPECIAL_WORD));
                        return;
                    }
                }
                else if (words[0] == mainRoute.Last() && words.Length > 1)
                {
                    if (airportList.Find(words.Last()) != null)
                    {
                        routeTo.Add(words.SubArray(0, words.Length - 1, SPECIAL_WORD));
                        return;
                    }
                    else
                    {
                        routeTo.Add(words.Exclude(SPECIAL_WORD));
                        return;
                    }
                }
            }
            throw new ArgumentException("Bad format.");
        }
    }
}
