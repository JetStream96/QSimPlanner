using System;
namespace QSP.RouteFinding.Tracks
{

    [Serializable()]
	public class TrackWaypointNotFoundException : System.Exception
	{
		public TrackWaypointNotFoundException()
		{
		}

		public TrackWaypointNotFoundException(string message) : base(message)
		{
		}

		public TrackWaypointNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}
	}


    [Serializable]
    public class TrackDownloadException : Exception
    {
        public TrackDownloadException() { }
        public TrackDownloadException(string message) : base(message) { }
        public TrackDownloadException(string message, Exception inner) : base(message, inner) { }
        protected TrackDownloadException(
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
