using QSP.FuelCalculation.FuelData;

namespace QSP.UI.Views.FuelPlan
{
    public interface IFuelPlanningView
    {
        FuelDataItem GetFuelData();
        double GetZfwTon();
        
        string OrigIcao { get; }
        string DestIcao { get; }
    }
}
