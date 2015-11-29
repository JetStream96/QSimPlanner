using System;
namespace QSP.RouteFinding.Tracks
{

    [Serializable()]
	public class TrackWaypointNotFoundException : System.Exception
	{

		public TrackWaypointNotFoundException()
		{
		}

		public TrackWaypointNotFoundException(string message) : base(message)
		{
		}

		public TrackWaypointNotFoundException(string message, Exception inner) : base(message, inner)
		{
		}

	}

}
