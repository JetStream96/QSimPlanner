using System.Collections.ObjectModel;
using QSP.RouteFinding.Tracks.Common;
using QSP.AviationTools.Coordinates;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacificTrack : Track
    {
        public PacotDirection Direction { get; private set; }

        public sealed override string AirwayIdent
        {
            get
            {
                return "PACOT" + Ident;
            }
        }

        public PacificTrack(PacotDirection Direction,
                            string Ident,
                            string TimeStart,
                            string TimeEnd, string Remarks,
                            ReadOnlyCollection<string> MainRoute,
                            ReadOnlyCollection<string[]> RouteFrom,
                            ReadOnlyCollection<string[]> RouteTo,
                            LatLon PreferredFirstLatLon)

             : base(Ident, TimeStart, TimeEnd, Remarks, MainRoute, RouteFrom, RouteTo, PreferredFirstLatLon)
        {
            this.Direction = Direction;
        }
    }
}
