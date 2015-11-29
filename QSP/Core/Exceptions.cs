using System;

namespace QSP.Core
{
    [Serializable()]
    public class InvalidAircraftDatabaseException : ApplicationException
    {

        public InvalidAircraftDatabaseException()
        {
        }

        public InvalidAircraftDatabaseException(string message) : base(message)
        {
        }

        public InvalidAircraftDatabaseException(string message, Exception inner) : base(message, inner)
        {
        }

    }


    [Serializable()]
    public class InvalidUserInputException : ApplicationException
    {

        public InvalidUserInputException()
        {
        }

        public InvalidUserInputException(string message) : base(message)
        {
        }

        public InvalidUserInputException(string message, Exception inner) : base(message, inner)
        {
        }

    }


    [Serializable()]
    public class RunwayTooShortException : System.Exception
    {

        public RunwayTooShortException()
        {
        }

        public RunwayTooShortException(string message) : base(message)
        {
        }

        public RunwayTooShortException(string message, Exception inner) : base(message, inner)
        {
        }
    }


    [Serializable()]
    public class PoorClimbPerformanceException : System.Exception
    {

        public PoorClimbPerformanceException()
        {
        }

        public PoorClimbPerformanceException(string message) : base(message)
        {
        }

        public PoorClimbPerformanceException(string message, Exception inner) : base(message, inner)
        {
        }
    }


    [Serializable()]
    public class EnumNotSupportedException : System.NotSupportedException
    {

        public EnumNotSupportedException()
        {
        }

        public EnumNotSupportedException(string message) : base(message)
        {
        }

        public EnumNotSupportedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
