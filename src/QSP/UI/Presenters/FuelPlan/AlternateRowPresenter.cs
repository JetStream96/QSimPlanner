using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.UI.Views.FuelPlan;
using QSP.UI.Views.Route;

namespace QSP.UI.Presenters.FuelPlan
{
    public class AlternateRowPresenter
    {
        private IAlternateRowView view;
        private Func<string> destIcaoGetter;
        private Func<AirportManager> airportListGetter;

        public FindAltnPresenter FindAltnPresenter(IFindAltnView altnView)=>
            new FindAltnPresenter(altnView, airportListGetter());

        public string DestIcao => destIcaoGetter();

        public AlternateRowPresenter(IAlternateRowView view, 
            Func<string> destIcaoGetter, Func<AirportManager> airportListGetter)
        {
            this.view = view;
            this.destIcaoGetter = destIcaoGetter;
            this.airportListGetter = airportListGetter;
        }
    }
}
