using System.Collections.ObjectModel;
using QSP.AviationTools.Coordinates;

namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class Track
    {
        public string Ident { get; private set; }
        public abstract string AirwayIdent { get; }
        public string TimeStart { get; private set; }
        public string TimeEnd { get; private set; }
        public string Remarks { get; private set; }

        public ReadOnlyCollection<string> MainRoute { get; private set; }
        public ReadOnlyCollection<string[]> RouteFrom { get; private set; }
        public ReadOnlyCollection<string[]> RouteTo { get; private set; }
        public LatLon PreferredFirstLatLon { get; private set; }

        public Track(string Ident,
                     string TimeStart,
                     string TimeEnd,
                     string Remarks,
                     ReadOnlyCollection<string> MainRoute,
                     ReadOnlyCollection<string[]> RouteFrom,
                     ReadOnlyCollection<string[]> RouteTo,
                     LatLon PreferredFirstLatLon)
        {
            this.Ident = Ident;
            this.TimeStart = TimeStart;
            this.TimeEnd = TimeEnd;
            this.Remarks = Remarks;
            this.MainRoute = MainRoute;
            this.RouteFrom = RouteFrom;
            this.RouteTo = RouteTo;
            this.PreferredFirstLatLon = PreferredFirstLatLon;
        }
    }
}
