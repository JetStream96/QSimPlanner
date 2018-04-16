using QSP.WindAloft;
using System;

namespace QSP.UI.Models.FuelPlan.Routes
{
    public class RouteFinderModel
    {
        public FuelPlanningModel FuelPlanningModel { get; set; }
        public Func<AvgWindCalculator> WindCalc { get; set; }
    }
}
