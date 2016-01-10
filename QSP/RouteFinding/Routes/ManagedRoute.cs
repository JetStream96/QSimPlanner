using QSP.Core;

namespace QSP.RouteFinding.Routes
{
    public class ManagedRoute : Route
    {
        private RouteToggler toggler;

        public ManagedRoute() : base()
        {
            toggler = new RouteToggler(links);
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
        /// A string represents the usual route text with the Nats display option.
        /// </summary>
        /// <exception cref="EnumNotSupportedException"></exception>
        public string ToString(bool ShowFirstWaypoint, bool ShowLastWaypoint, TracksDisplayOption para1)
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
