namespace QSP.UI.Models.FuelPlan
{
    public interface IRefreshForOptionChange
    {
        void RefreshForAirportListChange();
        void RefreshForNavDataLocationChange();
    }
}