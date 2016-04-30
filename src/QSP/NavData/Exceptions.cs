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
    public class WaypointFileReadException : Exception
    {
        public WaypointFileReadException() { }
        public WaypointFileReadException(string message) : base(message) { }
        public WaypointFileReadException(string message, Exception inner) : base(message, inner) { }
        protected WaypointFileReadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
    
    [Serializable]
    public class WaypointFileParseException : Exception
    {
        public WaypointFileParseException() { }
        public WaypointFileParseException(string message) : base(message) { }
        public WaypointFileParseException(string message, Exception inner) : base(message, inner) { }
        protected WaypointFileParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
