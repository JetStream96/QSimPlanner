using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks.Common;
using QSP.AviationTools;
using static QSP.RouteFinding.Tracks.Common.Utilities;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class IndividualAusotsParser
    {
        private string Ident;
        private string TimeStart;
        private string TimeEnd;
        private string Remarks;
        private bool Available;

        private string text;
        private AirportManager airportList;

        private static char[] Delimiters = { ' ', '\r', '\n', '\t' };
        private static char[] Delimiters2 = { '\r', '\n', '\t' };
        private static LatLon PreferredFirstLatLon = new LatLon(-25, 133);

        private bool connectionRoutesExist;
        private bool rmkExist;
        private string[] mainRoute;
        private List<string[]> routeFrom;
        private List<string[]> routeTo;

        public IndividualAusotsParser(string text) : this(text, RouteFindingCore.AirportList) { }

        /// <summary>
        /// Sample input:
        /// TDM TRK MY15 151112233001 
        /// 1511122330 1511131400 
        /// JAMOR IBABI LEC CALAR OOD 25S133E LEESA 20S128E 15S122E ATMAP 
        /// RTS/YMML ML H164 JAMOR 
        /// RMK/AUSOTS GROUP A
        /// 
        /// </summary>
        /// <param name="text"></param>
        public IndividualAusotsParser(string text, AirportManager airportList)
        {
            this.text = text;
            this.airportList = airportList;
        }

        public AusTrack Parse()
        {
            routeFrom = new List<string[]>();
            routeTo = new List<string[]>();

            parse();
            checkTrackExist();

            convertAllLatLonFormat();
            removeRedundentFromList();

            if (Available)
            {
                return new AusTrack(Ident,
                                    TimeStart,
                                    TimeEnd,
                                    Remarks,
                                     Array.AsReadOnly(mainRoute),
                                    routeFrom.AsReadOnly(),
                                    routeTo.AsReadOnly(),
                                    PreferredFirstLatLon);
            }
            else
            {
                return null;
            }
        }

        private void checkTrackExist()
        {
            Available = !(mainRoute.Contains(Ident) || mainRoute.Contains("-"));
        }

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
            if (rmkExist)
            {
                index += "RMK/".Length;
                Remarks = text.Substring(index);
            }
        }

        private void getMainRoute(ref int index)
        {
            int x = text.IndexOf("RTS/", index);

            if (x < 0)
            {
                x = text.IndexOf("RMK/", index);
                connectionRoutesExist = false;

                if (x < 0)
                {
                    x = text.Length - 1;
                }
            }
            else
            {
                connectionRoutesExist = true;
            }

            mainRoute = text.Substring(index, x - index).Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);
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

            var allRoutes = new List<string[]>();
            int nextLine = 0;

            while (index < x)
            {
                nextLine = text.IndexOf('\n', index + 1);

                if (nextLine == -1)
                {
                    nextLine = x;
                }

                allRoutes.Add(text.Substring(index, nextLine - index).Split(Delimiters, StringSplitOptions.RemoveEmptyEntries));
                index = nextLine;
            }

            for (int i = 0; i <= allRoutes.Count - 1; i++)
            {
                var rte = allRoutes[i];

                if (rte != null && rte.Count() > 1)
                {
                    if (rte.First() == mainRoute.Last())
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
                    else if (rte.Last() == mainRoute.First())
                    {
                        if (airportList.Find(rte.First()) != null)
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

        private string readToNextDelim(string item, ref int index)
        {
            int x = item.IndexOfAny(Delimiters, index);
            string str = item.Substring(index, x - index);
            index = x;
            return str;
        }

        private void skipConsectiveChars(string item, ref int index)
        {
            while (index < item.Length)
            {
                if (Delimiters.Contains(item[index]))
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
