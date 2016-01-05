using System.Collections.ObjectModel;
using QSP.AviationTools;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface ITrack
    {
        string Ident { get; }
        ReadOnlyCollection<string> MainRoute { get; }
        string TimeStart { get; }
        string TimeEnd { get; }
        ReadOnlyCollection<string[]> RouteFrom { get; }
        ReadOnlyCollection<string[]> RouteTo { get; }
        string Remarks { get; }

        LatLon PreferredFirstLatLon { get; }
    }
}
