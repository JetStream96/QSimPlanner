using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.UI.UserControls
{
    public class AlternateRowPresenter
    {
        private IAlternateRowView view;

        public AlternateRowPresenter(IAlternateRowView view, 
            Func<string> destIcaoGetter, Func<AirportManager> airportListGetter)
        {
            this.view = view;
        }
    }
}
