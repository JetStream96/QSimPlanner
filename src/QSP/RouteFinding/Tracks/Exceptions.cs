using System;

namespace QSP.RouteFinding.Tracks
{
    [Serializable()]
    public class TrackWaypointNotFoundException : Exception
    {
        public TrackWaypointNotFoundException() { }

        public TrackWaypointNotFoundException(string message) : base(message)
        { }

        public TrackWaypointNotFoundException(string message, Exception inner) : base(message, inner)
        { }
    }


    [Serializable]
    public class GetTrackException : Exception
    {
        public GetTrackException() { }
        public GetTrackException(string message) : base(message) { }
        public GetTrackException(string message, Exception inner) : base(message, inner) { }
        protected GetTrackException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class TrackParseException : Exception
    {
        public TrackParseException() { }
        public TrackParseException(string message) : base(message) { }
        public TrackParseException(string message, Exception inner) : base(message, inner) { }
        protected TrackParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
