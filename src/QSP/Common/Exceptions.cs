using System;

namespace QSP.Common
{
    [Serializable()]
    public class InvalidAircraftDatabaseException : ApplicationException
    {
        public InvalidAircraftDatabaseException() { }

        public InvalidAircraftDatabaseException(string message)
            : base(message)
        { }

        public InvalidAircraftDatabaseException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [Serializable()]
    public class InvalidUserInputException : ApplicationException
    {
        public InvalidUserInputException() { }

        public InvalidUserInputException(string message) : base(message)
        { }

        public InvalidUserInputException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [Serializable()]
    public class RunwayTooShortException : Exception
    {
        public RunwayTooShortException() { }

        public RunwayTooShortException(string message) : base(message)
        { }

        public RunwayTooShortException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [Serializable()]
    public class PoorClimbPerformanceException : Exception
    {

        public PoorClimbPerformanceException() { }

        public PoorClimbPerformanceException(string message) : base(message)
        { }

        public PoorClimbPerformanceException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [Serializable()]
    public class EnumNotSupportedException : NotSupportedException
    {
        public EnumNotSupportedException() { }

        public EnumNotSupportedException(string message) : base(message)
        { }

        public EnumNotSupportedException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    [Serializable]
    public class NoFileNameAvailException : Exception
    {
        public NoFileNameAvailException() { }
        public NoFileNameAvailException(string message) : base(message) { }
        public NoFileNameAvailException(string message, Exception inner)
            : base(message, inner)
        { }
        protected NoFileNameAvailException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
