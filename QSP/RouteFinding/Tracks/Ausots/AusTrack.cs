using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks.Common;
using System.Collections.ObjectModel;
using QSP.AviationTools;
using static QSP.RouteFinding.Tracks.Common.Utilities;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusTrack : ITrack
    {
        private string textMsg;
        private static char[] Delimiters = { ' ', '\r', '\n', '\t' };
        private static char[] Delimiters2 = { '\r', '\n', '\t' };

        private bool connectionRoutesExist;
        private bool rmkExist;
        private string[] _mainRoute;
        private List<string[]> _routeFrom;
        private List<string[]> _routeTo;

        /// <summary>
        /// Sample input:
        /// TDM TRK MY15 151112233001 
        /// 1511122330 1511131400 
        /// JAMOR IBABI LEC CALAR OOD 25S133E LEESA 20S128E 15S122E ATMAP 
        /// RTS/YMML ML H164 JAMOR 
        /// RMK/AUSOTS GROUP A
        /// 
        /// </summary>
        /// <param name="textMsg"></param>
        public AusTrack(string textMsg)
        {
            this.textMsg = textMsg;
            _routeFrom = new List<string[]>();
            _routeTo = new List<string[]>();

            parse();
            checkTrackExist();

            convertAllLatLonFormat();
            removeRedundentFromList();
        }

        #region "Properties"

        public string Ident { get; private set; }
        public string TimeStart { get; private set; }
        public string TimeEnd { get; private set; }

        public ReadOnlyCollection<string> MainRoute
        {
            get { return Array.AsReadOnly(_mainRoute); }
        }

        public ReadOnlyCollection<string[]> RouteFrom
        {
            get { return _routeFrom.AsReadOnly(); }
        }

        public ReadOnlyCollection<string[]> RouteTo
        {
            get { return _routeTo.AsReadOnly(); }
        }

        public string Remarks { get; private set; }

        public LatLon PreferredFirstLatLon
        {
            get { return new LatLon(-25, 133); }
        }

        public bool Available { get; private set; }

        #endregion

        private void checkTrackExist()
        {
            Available = !(_mainRoute.Contains(Ident) || _mainRoute.Contains("-"));
        }

        private void parse()
        {
            int index = 0;

            //get ident
            index = textMsg.IndexOf("TDM TRK", index);
            index += "TDM TRK".Length;
            skipConsectiveChars(textMsg, ref index);
            Ident = readToNextDelim(textMsg, ref index);

            //goto next line
            index = textMsg.IndexOf('\n', index) + 1;
            skipConsectiveChars(textMsg, ref index);
            TimeStart = readToNextDelim(textMsg, ref index);
            skipConsectiveChars(textMsg, ref index);
            TimeEnd = readToNextDelim(textMsg, ref index);

            //goto next line
            index = textMsg.IndexOf('\n', index) + 1;

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
                Remarks = textMsg.Substring(index);
            }
        }

        private void getMainRoute(ref int index)
        {
            int x = textMsg.IndexOf("RTS/", index);

            if (x < 0)
            {
                x = textMsg.IndexOf("RMK/", index);
                connectionRoutesExist = false;

                if (x < 0)
                {
                    x = textMsg.Length - 1;
                }

            }
            else
            {
                connectionRoutesExist = true;
            }

            _mainRoute = textMsg.Substring(index, x - index).Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);
            index = x;
        }

        private void gotoRmkSection(ref int index)
        {
            index = textMsg.IndexOf("RMK/", index);

            if (index < 0)
            {
                index = textMsg.Length - 1;
                rmkExist = false;
            }
            else
            {
                rmkExist = true;
            }
        }

        private void getConnectionRoute(ref int index)
        {
            int x = textMsg.IndexOf("RMK/", index);

            if (x < 0)
            {
                x = textMsg.Length - 1;
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
                nextLine = textMsg.IndexOf('\n', index + 1);

                if (nextLine == -1)
                {
                    nextLine = x;
                }

                allRoutes.Add(textMsg.Substring(index, nextLine - index).Split(Delimiters, StringSplitOptions.RemoveEmptyEntries));
                index = nextLine;
            }

            for (int i = 0; i <= allRoutes.Count - 1; i++)
            {
                var rte = allRoutes[i];

                if (rte != null && rte.Count() > 1)
                {
                    if (rte.First() == _mainRoute.Last())
                    {
                        //this route is routeTo
                        if (RouteFindingCore.AirportList.Find(rte.Last()) != null)
                        {
                            _routeTo.Add(rte.SubArray(0, rte.Length - 1));
                        }
                        else
                        {
                            _routeTo.Add(rte);
                        }
                    }
                    else if (rte.Last() == _mainRoute.First())
                    {
                        if (RouteFindingCore.AirportList.Find(rte.First()) != null)
                        {
                            _routeFrom.Add(rte.SubArray(1, rte.Length - 1));
                        }
                        else
                        {
                            _routeFrom.Add(rte);
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
            ConvertLatLonFormat(_mainRoute);

            foreach (var i in _routeFrom)
            {
                ConvertLatLonFormat(i);
            }
            foreach (var j in _routeTo)
            {
                ConvertLatLonFormat(j);
            }
        }

        private void removeRedundentFromList()
        {
            //choose distinct items
            _routeFrom = SelectDistinct(_routeFrom);
            _routeTo = SelectDistinct(_routeTo);

            //remove routes containing only 1 waypoint
            _routeFrom.RemoveTinyArray(2);
            _routeTo.RemoveTinyArray(2);
        }
    }
}
