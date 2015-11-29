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

}
