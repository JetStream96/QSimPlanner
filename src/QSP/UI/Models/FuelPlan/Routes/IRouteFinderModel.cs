using QSP.WindAloft;
using System;

namespace QSP.UI.Models.FuelPlan.Routes
{
    public interface IRouteFinderModel
    {
        IFuelPlanningModel FuelPlanningModel { get; }
        Func<AvgWindCalculator> WindCalc { get; }
    }

    public class RouteFinderModel : IRouteFinderModel
    {
        public IFuelPlanningModel FuelPlanningModel { get; }
        public Func<AvgWindCalculator> WindCalc { get; }

        public RouteFinderModel(IFuelPlanningModel FuelPlanningModel,
            Func<AvgWindCalculator> WindCalc)
        {
            this.FuelPlanningModel = FuelPlanningModel;
            this.WindCalc = WindCalc;
        }
    }
}
