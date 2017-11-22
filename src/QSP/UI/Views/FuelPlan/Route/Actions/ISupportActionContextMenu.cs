using Routes = QSP.RouteFinding.Routes;

namespace QSP.UI.Views.FuelPlan.Route.Actions
{
    /// <summary>
    /// This inteface should be implemented for any view that supports ActionContextMenu.
    /// </summary>
    public interface ISupportActionContextMenu : IMessageDisplay
    {
        string DistanceInfo { set; }
        string Route { get; set; }

        void ShowMap(Routes.Route route);
        void ShowMapBrowser(Routes.Route route);
    }
}