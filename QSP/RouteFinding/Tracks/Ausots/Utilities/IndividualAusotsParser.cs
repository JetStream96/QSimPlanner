using QSP.AviationTools;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.StringParser.Utilities;
using static QSP.RouteFinding.Tracks.Common.Utilities;

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
    // Notes:
    // (1) Ident, TimeStart, and TimeEnd must exist.
    // (2) All LatLon format is assumed to be 7-digit (like 25S133E), and will all be converted to 5-digit.
    // (3) RTS/ and RMK/ may not exist.

    public class IndividualAusotsParser
    {
        private static readonly LatLon PreferredFirstLatLon = new LatLon(-25.0, 133.0);

        private string text;
        private AirportManager airportList;

        private string Ident;
        private string TimeStart;
        private string TimeEnd;
        private string Remarks;
        private bool connectionRoutesExist;
        private bool rmkExist;
        private string[] mainRoute;
        private List<string[]> routeFrom;
        private List<string[]> routeTo;

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
            routeFrom = new List<string[]>();
            routeTo = new List<string[]>();

            try
            {
                parse();
            }
            catch
            {
                throw new TrackParseException();
            }
            
            if (TrackAvailble())
            {
                convertAllLatLonFormat();
                removeRedundentFromList();

                return new AusTrack(Ident,
                                    TimeStart,
                                    TimeEnd,
                                    Remarks,
                                    Array.AsReadOnly(mainRoute),
                                    routeFrom.AsReadOnly(),
                                    routeTo.AsReadOnly(),
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
        private bool TrackAvailble()
        {
            return !(mainRoute.Contains(Ident) || mainRoute.Contains("-"));
        }

        // Exception may occur if the input string format is not as expected (especially IndexOutOfRangeException).
        private void parse()
        {
            int index = 0;

            //get ident
            index = text.IndexOf("TDM TRK", index);
            index += "TDM TRK".Length;
            skipConsectiveChars(text, ref index);
            Ident = readToNextDelim(text, ref index);

            //goto next line
            index = text.IndexOf('\n', index) + 1;
            skipConsectiveChars(text, ref index);
            TimeStart = readToNextDelim(text, ref index);
            skipConsectiveChars(text, ref index);
            TimeEnd = readToNextDelim(text, ref index);

            //goto next line
            index = text.IndexOf('\n', index) + 1;

            //get everything before "RTS/", except for special chars
            getMainRoute(ref index);

            //get everything before "RMK/", except for special chars
            if (connectionRoutesExist)
            {
                index += "RTS/".Length;
                getConnectionRoute(ref index);
            }
            else
            {
                gotoRmkSection(ref index);
            }

            //get remarks
            index = getRemarks(index);
        }

        private int getRemarks(int index)
        {
            if (rmkExist)
            {
                index += "RMK/".Length;
                Remarks = text.Substring(index);
            }
            else
            {
                Remarks = "";
            }

            return index;
        }

        private void getMainRoute(ref int index)
        {
            int x = text.IndexOf("RTS/", index);

            if (x < 0)
            {
                // RTS/ may not exist.

                x = text.IndexOf("RMK/", index);
                connectionRoutesExist = false;

                if (x < 0)
                {
                    // RMK/ may not exist.
                    x = text.Length - 1;
                }
            }
            else
            {
                connectionRoutesExist = true;
            }

            mainRoute = text.Substring(index, x - index).Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);
            index = x;
        }

        private void gotoRmkSection(ref int index)
        {
            index = text.IndexOf("RMK/", index);

            if (index < 0)
            {
                index = text.Length - 1;
                rmkExist = false;
            }
            else
            {
                rmkExist = true;
            }
        }

        // In AUSOTs TDM, a connection route (route specified after RTS/), may look like:
        // (1) TAM V327 HAWKE Y491 SMOKA Y177 BN YBBN
        // (2) BEBAK OK AGIVA PARRY Y195 GLENN BN YBBN
        // (3) YSSY TESAT H202 RIC UH226 NYN
        private void getConnectionRoute(ref int index)
        {
            int x = text.IndexOf("RMK/", index);

            if (x < 0)
            {
                x = text.Length - 1;
                rmkExist = false;
            }
            else
            {
                rmkExist = true;
            }

            // There may be multiple connecting routes. E.g.
            // ...
            // RTS /YBBN BN H62 LAV Q116 TAXEG 
            // NSM Q10 HAMTN Q158 PH YPPH
            // RMK / AUSOTS GROUP E
            //
            // There are 2 connection routes in the above example.

            var allRoutes = new List<string[]>();
            int nextLine = 0;

            while (index < x)
            {
                nextLine = text.IndexOf('\n', index + 1);

                if (nextLine == -1)
                {
                    nextLine = x;
                }

                allRoutes.Add(text.Substring(index, nextLine - index).Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries));
                index = nextLine;
            }

            for (int i = 0; i <= allRoutes.Count - 1; i++)
            {
                var rte = allRoutes[i];

                if (rte != null && rte.Count() > 1)
                {
                    if (rte[0] == mainRoute.Last())
                    {
                        //this route is routeTo
                        if (airportList.Find(rte.Last()) != null)
                        {
                            routeTo.Add(rte.SubArray(0, rte.Length - 1));
                        }
                        else
                        {
                            routeTo.Add(rte);
                        }
                    }
                    else if (rte.Last() == mainRoute[0])
                    {
                        if (airportList.Find(rte[0]) != null)
                        {
                            routeFrom.Add(rte.SubArray(1, rte.Length - 1));
                        }
                        else
                        {
                            routeFrom.Add(rte);
                        }
                    }
                }
            }
            index = x;
        }

        private static string readToNextDelim(string item, ref int index)
        {
            int x = item.IndexOfAny(DelimiterWords, index);
            string str = item.Substring(index, x - index);
            index = x;
            return str;
        }

        private static void skipConsectiveChars(string item, ref int index)
        {
            while (index < item.Length)
            {
                if (DelimiterWords.Contains(item[index]))
                {
                    index++;
                }
                else
                {
                    return;
                }
            }
        }

        private void convertAllLatLonFormat()
        {
            ConvertLatLonFormat(mainRoute);

            foreach (var i in routeFrom)
            {
                ConvertLatLonFormat(i);
            }
            foreach (var j in routeTo)
            {
                ConvertLatLonFormat(j);
            }
        }

        private void removeRedundentFromList()
        {
            //choose distinct items
            routeFrom = SelectDistinct(routeFrom);
            routeTo = SelectDistinct(routeTo);

            //remove routes containing only 1 waypoint
            routeFrom.RemoveTinyArray(2);
            routeTo.RemoveTinyArray(2);
        }
    }
}
