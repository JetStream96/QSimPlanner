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
    // Format:
    // TDM TRK [Ident] 
    // [TimeStart] [TimeEnd]
    // [Main Route]
    // [Main Route continued]
    // RTS/[RouteFrom/To 1]
    // [RouteFrom/To 2]
    // [RouteFrom/To 3]
    // RMK/[Remarks]
    // [Remarks continued]
    //
    //    
    // Assumptions:
    // (1) The word after "TDM TRK" is the track ident (MY15 in above example).
    //
    // (2) The next line contains two time stamps, indicating TimeStart and TimeEnd respectively.
    //
    // (3) The main route starts from the next line, to:
    //        a. The char before "RTS/" if exists.
    //        b. The char before "RMK/" if "RTS/" does not exist but "RMK/" exists.
    //        c. The last char of the string if neither "RMK/" nor "RTS/" exists.
    //
    // (4) After main route, the connecting routes contains the part after "RTS/", to:
    //        a. The char before "RMK/" if exists.
    //        b. The last char of the string if "RMK/" does not exist. 
    // 
    // (5) If "RTS/" does not exist the RouteFrom/To is empty.
    //
    // (6) After connecting routes, the remarks contains the part after "RMK/", to the end of string.
    //     If "RMK/" does not exist the remarks is am empty string.   
    //
    // (7) All LatLon format is assumed to be 7-digit (like 25S133E), and will all be converted to 5-digit.

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
            SkipAny(text, DelimiterWords, ref index);
            Ident = ReadToNextDelimeter(text,DelimiterWords, ref index);

            //goto next line
            SkipToNextLine(text, ref index);
            SkipAny(text, DelimiterWords, ref index);
            TimeStart = ReadToNextDelimeter(text, DelimiterWords, ref index);
            SkipAny(text, DelimiterWords, ref index);
            TimeEnd = ReadToNextDelimeter(text, DelimiterWords, ref index);

            //goto next line
            SkipToNextLine(text, ref index);

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
