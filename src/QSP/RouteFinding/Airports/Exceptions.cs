using System;

namespace QSP.RouteFinding.Airports
{

    [Serializable]
    public class ReadAirportDBFileException : Exception
    {
        public ReadAirportDBFileException() { }
        public ReadAirportDBFileException(string message) : base(message) { }
        public ReadAirportDBFileException(string message, Exception inner) : base(message, inner) { }
        protected ReadAirportDBFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
