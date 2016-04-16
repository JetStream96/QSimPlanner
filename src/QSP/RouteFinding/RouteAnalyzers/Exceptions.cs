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


    [Serializable]
    public class WaypointTooFarException : Exception
    {   
        public WaypointTooFarException() { }
        public WaypointTooFarException(string message) : base(message) { }
        public WaypointTooFarException(string message, Exception inner) : base(message, inner) { }
        protected WaypointTooFarException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
