using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;
using static QSP.LibraryExtension.StringParser.Utilities;
using static QSP.RouteFinding.Tracks.Common.Utilities;
using QSP.LibraryExtension.StringParser;

namespace QSP.RouteFinding.Tracks.Common.Parser
{
    // Parse the string which represents a single track, into a temporary format.
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

    public class ParserTDMOld
    {
        private string text;
        private AirportManager airportList;

        private bool rtsExist;
        private bool rmkExist;

        private int rtsIndex;
        private int rmkIndex;

        private string Ident;
        private string TimeStart;
        private string TimeEnd;
        private string Remarks;
        private string[] mainRoute;
        private List<string[]> routeFrom;
        private List<string[]> routeTo;

        public ParserTDMOld(string text, AirportManager airportList)
        {
            this.text = text;
            this.airportList = airportList;
        }

        /// <exception cref="TrackParseException"></exception>
        public ParseResultOld Parse()
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

            convertAllLatLonFormat();
            removeRedundentFromList();

            return new ParseResultOld(Ident,
                                   TimeStart,
                                   TimeEnd,
                                   Remarks,
                                   mainRoute,
                                   routeFrom,
                                   routeTo);

        }

        // Exception may occur if the input string format is not as expected (especially IndexOutOfRangeException).
        private void parse()
        {
            var sp = new StringParser(text);
            var d = DelimiterWords;

            sp.MoveToNextIndexOf("TDM TRK");
            sp.MoveRight("TDM TRK".Length);
            sp.SkipAny(d);

            Ident = sp.ReadToNextDelimeter(d);

            sp.SkipToNextLine();
            sp.SkipAny(d);

            TimeStart = sp.ReadToNextDelimeter(d);
            sp.SkipAny(d);
            TimeEnd = sp.ReadToNextDelimeter(d);

            sp.SkipToNextLine();
            mainRoute=sp.ReadString(mainRouteEndIndex()).Split(d, StringSplitOptions.RemoveEmptyEntries);

            if (rtsIndex >= 0)
            {
                sp.MoveRight("RTS/".Length);

            }
            

            int index = sp.CurrentIndex;
            
            //get everything before "RMK/", except for special chars
            if (rtsExist)
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

        private void getRtsRmkIndices(int index)
        {
            rtsIndex = text.IndexOf("RTS/", index);
            rmkIndex = text.IndexOf("RMK/", index);
        }

        private int mainRouteEndIndex()
        {
            int EndIndexPlusOne;

            if (rtsIndex >= 0)
            {
                EndIndexPlusOne = rtsIndex;
            }
            else if (rmkIndex >= 0)
            {
                EndIndexPlusOne = rmkIndex;
            }
            else
            {
                EndIndexPlusOne = text.Length;
            }
            return EndIndexPlusOne - 1;
        }

        private int rtsEndIndex()
        {
            int EndIndexPlusOne;

            if (rmkIndex >= 0)
            {
                EndIndexPlusOne = rmkIndex;
            }
            else
            {
                EndIndexPlusOne = text.Length;
            }
            return EndIndexPlusOne - 1;
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

            for (int i = 0; i < allRoutes.Count; i++)
            {
                var rte = allRoutes[i];

                if (rte != null && rte.Length > 1)
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
