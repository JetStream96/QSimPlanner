using QSP.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.UI.UserControls
{
    public interface IFuelPlanningView
    {
        IEnumerable<string> AircraftList { set; }
        IEnumerable<string> RegistrationList { set; }
        WeightUnit WeightUnit { set; }
        double O
    }
}
