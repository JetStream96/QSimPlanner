using QSP.AviationTools;
using QSP.RouteFinding.Airports;
using QSP.UI.Views.FuelPlan.Routes;
using System.Collections.ObjectModel;

namespace QSP.UI.Presenters
{
    public class FindAltnPresenter
    {
        private IFindAltnView view;
        private AirportManager airportList;

        public FindAltnPresenter(IFindAltnView view, AirportManager airportList)
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

        /// <summary>
        /// If the ICAO is found and the minimum runway length is a valid number,
        /// update the list of alternates. Otherwise, clear the list of alternates.
        /// </summary>
        public void FindAlternates()
        {
            view.Alternates = new AlternateFinder.AlternateInfo[0];
            double? lengthFt = TryGetLengthFt();

            if (lengthFt == null) return;

            var icao = view.ICAO;
            if (airportList[icao] == null) return;

            var finder = new AlternateFinder(airportList);
            view.Alternates = new ReadOnlyCollection<AlternateFinder.AlternateInfo>(
                finder.AltnInfo(icao, (int)lengthFt));
        }
    }
}
