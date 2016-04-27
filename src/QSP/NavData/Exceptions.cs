using System;

namespace QSP.NavData
{
    [Serializable]
    public class ReadAirportFileException : Exception
    {
        public ReadAirportFileException() { }
        public ReadAirportFileException(string message) : base(message) { }
        public ReadAirportFileException(string message, Exception inner) : base(message, inner) { }
        protected ReadAirportFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class RwyDataFormatException : Exception
    {
        public RwyDataFormatException() { }
        public RwyDataFormatException(string message) : base(message) { }
        public RwyDataFormatException(string message, Exception inner) : base(message, inner) { }
        protected RwyDataFormatException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class LoadWaypointFileException : Exception
    {
        public LoadWaypointFileException() { }
        public LoadWaypointFileException(string message) : base(message) { }
        public LoadWaypointFileException(string message, Exception inner) : base(message, inner) { }
        protected LoadWaypointFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
