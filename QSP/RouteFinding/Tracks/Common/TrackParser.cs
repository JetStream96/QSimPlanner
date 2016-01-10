using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class TrackParser<T> where T : ITrack
    {
        public abstract List<T> Parse();
    }    
}