namespace QSP.RouteFinding.Tracks.Nats
{
    public interface INatsMessageProvider
    {
        /// <exception cref="GetTrackException"></exception>
        NatsMessage GetMessage();
    }
}
