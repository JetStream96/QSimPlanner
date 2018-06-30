using QSP.RouteFinding.Routes;

namespace QSP.UI.Views.FuelPlan.Routes.Actions
{
    /// <summary>
    /// This inteface should be implemented for any view that supports ActionContextMenu.
    /// </summary>
    public interface ISupportActionContextMenu : IMessageDisplay
    {
        string DistanceInfo { set; }
        string Route { get; set; }

        void ShowMap(Route route);

        /// <summary>
        /// This should be set to true if the Route has been analyzed,
        /// or the Route was generated.
        /// </summary>
        bool RouteIsValid { set; }
    }
}