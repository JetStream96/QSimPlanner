namespace QSP.RouteFinding.Tracks.Ausots
{
    public interface IAusotsMessageProvider
    {
        /// <exception cref="TrackParseException"></exception>
        AusotsMessage GetMessage();
    }
}
