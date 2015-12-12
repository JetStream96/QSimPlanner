using System;
namespace QSP.RouteFinding
{

    [Serializable()]
    public class SidNotFoundException : System.Exception
    {

        public SidNotFoundException()
        {
        }

        public SidNotFoundException(string message) : base(message)
        {
        }

        public SidNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    [Serializable()]
    public class WaypointNotFoundException : System.Exception
    {

        public WaypointNotFoundException()
        {
        }

        public WaypointNotFoundException(string message) : base(message)
        {
        }

        public WaypointNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    [Serializable()]
    public class RwyDatabaseFormatException : System.Exception
    {

        public RwyDatabaseFormatException(string message) : base(message)
        {
        }

        public RwyDatabaseFormatException(string message, Exception inner) : base(message, inner)
        {
        }

    }

    [Serializable()]
    public class InvalidIdentifierException : Exception
    {

        public InvalidIdentifierException()
        {
        }

        public InvalidIdentifierException(string message) : base(message)
        {
        }

        public InvalidIdentifierException(string message, Exception inner) : base(message, inner)
        {
        }
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


    [Serializable]
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException() { }
        public RouteNotFoundException(string message) : base(message) { }
        public RouteNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected RouteNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }


}
