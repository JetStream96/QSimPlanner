namespace QSP.RouteFinding.Tracks.Pacots
{
    public interface IPacotsMessageProvider
    {
        /// <exception cref="GetTrackException"></exception>
        PacotsMessage GetMessage();
    }
}
