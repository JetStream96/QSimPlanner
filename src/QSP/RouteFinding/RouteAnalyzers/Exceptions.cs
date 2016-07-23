using System;

namespace QSP.RouteFinding.RouteAnalyzers
{
    [Serializable]
    public class InvalidRouteException : Exception
    {
        public InvalidRouteException() { }
        public InvalidRouteException(string message) : base(message) { }
        public InvalidRouteException(string message, Exception inner) : base(message, inner) { }
        protected InvalidRouteException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }    
}
