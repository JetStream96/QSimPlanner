using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.LibraryExtension;
using static QSP.LibraryExtension.Arrays;
using System.Collections.ObjectModel;
using QSP.AviationTools;
using QSP.RouteFinding.Tracks.Common;
using QSP.Core;
using static QSP.RouteFinding.Tracks.Pacots.Constants;
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

        public WestTrackParser(string text,AirportManager airportList)
        {
            routeFrom = new List<string[]>();
            routeTo = new List<string[]>();
            this.text = text;
            this.airportList = airportList;
        }

        public PacificTrack Parse()
        {
            parseWest(text);
            removeRedundentFromList();
            convertAllLatLonFormat();

            return new PacificTrack(Ident,
                                    PacotDirection.Westbound,
                                    TimeStart,
                                    TimeEnd,
                                    Array.AsReadOnly(mainRoute),
                                    routeFrom.AsReadOnly(),
                                    routeTo.AsReadOnly(),
                                    Remarks);
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

        private void parseWest(string message)
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
            int x = message.IndexOf("(TDM TRK") + "(TDM TRK".Length;
            int y = message.IndexOf(' ', x);
            x = message.IndexOf(' ', y + 1);
            Ident = message.StringBetween(y, x);

            //get time start/end
            x = message.IndexOfAny(DelimiterLines, x);
            y = message.IndexOf(' ', x);
            TimeStart = message.StringBetween(x, y);
            x = message.IndexOfAny(DelimiterLines, y);
            TimeEnd = message.StringBetween(y, x);

            //get main route
            //TODO: what if RTS or RMK does not exist?
            y = message.IndexOf("RTS/", x);
            mainRoute = message.StringBetween(x, y).Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);

            //get route from/to
            y += "RTS/".Length - 1;

            int z = message.IndexOf("RMK/");

            while (y < z)
            {
                x = y;
                y = message.IndexOfAny(DelimiterLines, x + 1);

                if (y < z)
                {
                    addTrack(message.StringBetween(x, y));
                }
            }

            //get remarks
            Remarks = message.Substring(z + "RMK/".Length);
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
