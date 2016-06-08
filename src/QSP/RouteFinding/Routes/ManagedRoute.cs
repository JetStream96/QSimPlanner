using QSP.Common;
using QSP.RouteFinding.Routes.Toggler;

namespace QSP.RouteFinding.Routes
{
    public class ManagedRoute : Route
    {
        private RouteToggler toggler;

        public ManagedRoute(TrackInUseCollection tracksInUse) : base()
        {
            toggler = new RouteToggler(links, tracksInUse);
        }

        public ManagedRoute(Route route, TrackInUseCollection tracksInUse)
            : base(route)
        {
            toggler = new RouteToggler(links, tracksInUse);
        }

        /// <summary>
        /// Collapse the tracks for the route, if not done already.  
        /// </summary>
        public void Collapse()
        {
            toggler.Collapse();
        }

        /// <summary>
        /// Expand the Tracks for the route, if not already expanded. 
        /// </summary>
        public void Expand()
        {
            toggler.Expand();
        }

        public enum TracksDisplayOption
        {
            Expand,
            Collapse
        }

        /// <summary>
        /// A string represents the usual route text with 
        /// the Nats display option.
        /// </summary>
        /// <exception cref="EnumNotSupportedException"></exception>
        public string ToString(
            bool ShowFirstWaypoint,
            bool ShowLastWaypoint,
            TracksDisplayOption para1)
        {
            switch (para1)
            {
                case TracksDisplayOption.Expand:
                    Expand();
                    break;

                case TracksDisplayOption.Collapse:
                    Collapse();
                    break;

                default:
                    throw new EnumNotSupportedException("Incorrect enum for NatsDisplayOption.");
            }

            return ToString(ShowFirstWaypoint, ShowLastWaypoint);
        }
    }
}
