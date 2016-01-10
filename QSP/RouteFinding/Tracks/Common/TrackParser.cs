namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class TrackParser<T> where T : ITrack
    {
        public abstract T Parse();
    }    
}