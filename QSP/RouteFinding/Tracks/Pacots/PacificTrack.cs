using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;
using System.Collections.ObjectModel;
using QSP.AviationTools;
using QSP.RouteFinding.Tracks.Common;
using QSP.Core;

namespace QSP.RouteFinding.Tracks.Pacots
{

    public class PacificTrack : ITrack
    {

        #region "Fields"

        private string[] _mainRoute;
        private List<string[]> _routeFrom;
        private List<string[]> _routeTo;

        private static char[] CHANGELINE_CHAR = { '\r', '\n' };
        private static string[] Delimiters = { " ", "\r\n", "\r", "\n", "\t" };
        //UPR means upper (airway). This should be ignored when parsing routeFrom/To.
        private static string[] SPECIAL_WORD = { "UPR" };

        private static readonly LatLon JAPAN_LATLON = new LatLon(38, 147);
        private static readonly LatLon US_LATLON = new LatLon(41, -138);

        #endregion

        #region "Properties"

        public string Ident { get; private set; }
        public PacotDirection Direction { get; private set; }

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

        public string TimeStart { get; private set; }
        public string TimeEnd { get; private set; }
        public string Remarks { get; private set; }

        public LatLon PreferredFirstLatLon
        {
            get
            {
                switch (Direction)
                {
                    case PacotDirection.Eastbound:
                        return JAPAN_LATLON;

                    case PacotDirection.Westbound:
                        return US_LATLON;

                    default:
                        throw new EnumNotSupportedException();
                }
            }
        }

        public string AirwayIdent
        {
            get
            {
                return "PACOT" + Ident;
            }
        }

        #endregion

        public PacificTrack(string trackMsg, PacotDirection direction)
        {
            _routeFrom = new List<string[]>();
            _routeTo = new List<string[]>();
            this.Direction = direction;

            switch (direction)
            {
                case PacotDirection.Eastbound:

                    throw new ArgumentException("This constructor is only for westbound tracks.");
                case PacotDirection.Westbound:
                    parseWest(trackMsg);

                    break;
                default:

                    throw new ArgumentException("Invalid direction for PACOTs.");
            }

            removeRedundentFromList();
            convertAllLatLonFormat();

        }

        public PacificTrack(string ident, PacotDirection direction, string timeStart, string timeEnd, string[] mainRoute,
                            List<string[]> routeFrom, List<string[]> routeTo, string remark)
        {
            this.Ident = ident;
            this.Direction = direction;
            this.TimeStart = timeStart;
            this.TimeEnd = timeEnd;
            this._mainRoute = mainRoute;
            this._routeFrom = routeFrom;
            this._routeTo = routeTo;
            this.Remarks = remark;

            removeRedundentFromList();
            convertAllLatLonFormat();
        }

        private void convertAllLatLonFormat()
        {
            Common.Utilities.ConvertLatLonFormat(_mainRoute);

            foreach (var i in _routeFrom)
            {
                Common.Utilities.ConvertLatLonFormat(i);
            }

            foreach (var j in _routeTo)
            {
                Common.Utilities.ConvertLatLonFormat(j);
            }
        }

        private void removeRedundentFromList()
        {
            //choose distinct items
            _routeFrom = Common.Utilities.SelectDistinct(_routeFrom);
            _routeTo = Common.Utilities.SelectDistinct(_routeTo);

            //remove routes containing only 1 waypoint
            _routeFrom.RemoveTinyArray(2);
            _routeTo.RemoveTinyArray(2);
        }

        #region "Parse West"

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
            x = message.IndexOfAny(CHANGELINE_CHAR, x);
            y = message.IndexOf(' ', x);
            TimeStart = message.StringBetween(x, y);
            x = message.IndexOfAny(CHANGELINE_CHAR, y);
            TimeEnd = message.StringBetween(y, x);

            //get main route
            //TODO: what if RTS or RMK does not exist?
            y = message.IndexOf("RTS/", x);
            _mainRoute = message.StringBetween(x, y).Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);

            //get route from/to
            y += "RTS/".Length - 1;

            int z = message.IndexOf("RMK/");

            while (y < z)
            {
                x = y;
                y = message.IndexOfAny(CHANGELINE_CHAR, x + 1);

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
            var words = line.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {

                if (words.Last() == MainRoute.First())
                {
                    if (RouteFindingCore.AirportList.Find(words[0]) != null)
                    {
                        _routeFrom.Add(words.SubArray(1, words.Length - 1, SPECIAL_WORD));
                        return;
                    }
                    else
                    {
                        _routeFrom.Add(words.Exclude(SPECIAL_WORD));
                        return;
                    }


                }
                else if (words[0] == MainRoute.Last() && words.Length > 1)
                {
                    if (RouteFindingCore.AirportList.Find(words.Last()) != null)
                    {
                        _routeTo.Add(words.SubArray(0, words.Length - 1, SPECIAL_WORD));
                        return;
                    }
                    else
                    {
                        _routeTo.Add(words.Exclude(SPECIAL_WORD));
                        return;
                    }
                }
            }
            throw new ArgumentException("Bad format.");
        }

        #endregion

    }
}
