using QSP.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.FuelCalculation.FuelData;

namespace QSP.UI.Views.FuelPlan
{
    public interface IFuelPlanningView
    {
        FuelDataItem GetFuelData();
        double GetZfwTon();

        // IEnumerable<string> AircraftList { set; }
        //  IEnumerable<string> RegistrationList { set; }
        // WeightUnit WeightUnit { set; }
        // double OewKg { set; }
        // double MaxZfwKg { set; }

        string OrigIcao { get; }
        string DestIcao { get; }

        // Can be "AUTO".
        //  IEnumerable<string> OrigRwyList { set; }
        // IEnumerable<string> DestRwyList { set; }

        // Can be "AUTO" or "NONE".
        // IEnumerable<string> SidList { set; }
        // IEnumerable<string> StarList { set; }

        // string Route { get; set; }

    }
}
