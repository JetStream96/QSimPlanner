using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface ITrackParser<T> where T : Track
    {
        List<T> Parse();
    }
}