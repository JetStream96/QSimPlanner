namespace QSP.UI.Models.FuelPlan
{
    public interface IRefreshForNavDataChange
    {
        /// <summary>
        /// This method should be called after nav data changed.
        /// </summary>
        void OnNavDataChange();
    }
}