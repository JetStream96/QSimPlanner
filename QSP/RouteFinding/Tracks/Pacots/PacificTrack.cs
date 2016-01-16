using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;
using System.Collections.ObjectModel;
using QSP.AviationTools;
using QSP.RouteFinding.Tracks.Common;
using QSP.Core;
using static QSP.RouteFinding.Tracks.Pacots.Constants;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacificTrack : ITrack
    {      
        #region "Properties"

        public string Ident { get; private set; }
        public PacotDirection Direction { get; private set; }
        public ReadOnlyCollection<string> MainRoute { get; private set; }
        public ReadOnlyCollection<string[]> RouteFrom { get; private set; }
        public ReadOnlyCollection<string[]> RouteTo { get; private set; }
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

        public PacificTrack(string Ident, 
                            PacotDirection Direction, 
                            string TimeStart, 
                            string TimeEnd, 
                            ReadOnlyCollection<string> MainRoute,
                            ReadOnlyCollection<string[]> RouteFrom,
                            ReadOnlyCollection<string[]> RouteTo, 
                            string Remarks)
        {
            this.Ident = Ident;
            this.Direction = Direction;
            this.TimeStart = TimeStart;
            this.TimeEnd = TimeEnd;
            this.MainRoute = MainRoute;
            this.RouteFrom = RouteFrom;
            this.RouteTo = RouteTo;
            this.Remarks = Remarks;
        }
    }
}
