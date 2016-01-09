using System.Collections.ObjectModel;
using QSP.AviationTools;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface ITrack
    {
        string Ident { get; }
        string AirwayIdent { get; }
        string TimeStart { get; }
        string TimeEnd { get; }
        string Remarks { get; }
        ReadOnlyCollection<string> MainRoute { get; }
        ReadOnlyCollection<string[]> RouteFrom { get; }
        ReadOnlyCollection<string[]> RouteTo { get; }

        LatLon PreferredFirstLatLon { get; }
        
    }
}
