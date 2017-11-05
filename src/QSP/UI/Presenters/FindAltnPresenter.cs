using QSP.AviationTools;
using QSP.RouteFinding.Airports;
using System.Collections.ObjectModel;
using QSP.UI.Views.Route;

namespace QSP.UI.Presenters
{
    public class FindAltnPresenter
    {
        private IAltnView view;
        private AirportManager airportList;

        public FindAltnPresenter(IAltnView view, AirportManager airportList)
        {
            this.view = view;
            this.airportList = airportList;
        }

        private double? TryGetLengthFt()
        {
            if (double.TryParse(view.Length, out var length))
            {
                return ConversionTools.GetLengthFt(length, view.LengthUnit);
            }

            return null;
        }

        public void FindAlternates()
        {
            double? lengthFt = TryGetLengthFt();

            if (lengthFt == null)
            {
                view.ShowWarning("Invalid runway length.");
                return;
            }

            var icao = view.ICAO;
            if (airportList[icao] == null)
            {
                view.ShowWarning($"ICAO '{icao}' cannot be found.");
                return;
            }

            var finder = new AlternateFinder(airportList);
            view.Alternates = new ReadOnlyCollection<AlternateFinder.AlternateInfo>(
                finder.AltnInfo(icao, (int)lengthFt));
        }
    }
}
