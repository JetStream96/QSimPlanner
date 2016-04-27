using System;

namespace QSP.RouteFinding
{
    [Serializable]
    public class SidNotFoundException : Exception
    {
        public SidNotFoundException() { }
        public SidNotFoundException(string message) : base(message) { }
        public SidNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected SidNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class StarNotFoundException : Exception
    {
        public StarNotFoundException() { }
        public StarNotFoundException(string message) : base(message) { }
        public StarNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected StarNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class WaypointNotFoundException : Exception
    {
        public WaypointNotFoundException() { }
        public WaypointNotFoundException(string message) : base(message) { }
        public WaypointNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected WaypointNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
    
    [Serializable]
    public class InvalidIdentifierException : Exception
    {
        public InvalidIdentifierException() { }
        public InvalidIdentifierException(string message) : base(message) { }
        public InvalidIdentifierException(string message, Exception inner) : base(message, inner) { }
        protected InvalidIdentifierException(
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

    [Serializable]
    public class LoadSidFileException : Exception
    {
        public LoadSidFileException() { }
        public LoadSidFileException(string message) : base(message) { }
        public LoadSidFileException(string message, Exception inner) : base(message, inner) { }
        protected LoadSidFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    [Serializable]
    public class LoadStarFileException : Exception
    {
        public LoadStarFileException() { }
        public LoadStarFileException(string message) : base(message) { }
        public LoadStarFileException(string message, Exception inner) : base(message, inner) { }
        protected LoadStarFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}
