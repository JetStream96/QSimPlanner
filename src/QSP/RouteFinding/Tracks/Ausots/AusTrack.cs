using QSP.RouteFinding.Tracks.Common;
using System.Collections.ObjectModel;
using QSP.AviationTools.Coordinates;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusTrack : Track
    {
        public override string AirwayIdent
        {
            get
            {
                return "AUSOT" + Ident;
            }
        }

        public AusTrack(string Ident,
                        string TimeStart,
                        string TimeEnd,
                        string Remarks,
                        ReadOnlyCollection<string> MainRoute,
                        ReadOnlyCollection<string[]> RouteFrom,
                        ReadOnlyCollection<string[]> RouteTo,
                        LatLon PreferredFirstLatLon)

            : base(Ident, TimeStart, TimeEnd, Remarks, MainRoute, RouteFrom, RouteTo, PreferredFirstLatLon)
        { }
    }
}
