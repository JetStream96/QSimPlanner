namespace QSP.UI.Models.FuelPlan
{
    // TODO: why we need two instead one method here?
    public interface IRefreshForOptionChange
    {
        void RefreshForAirportListChange();
        void RefreshForNavDataLocationChange();
    }
}