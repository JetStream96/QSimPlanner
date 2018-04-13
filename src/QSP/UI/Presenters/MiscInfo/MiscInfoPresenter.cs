using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.UI.Views.MiscInfo;
using QSP.WindAloft;
using System;
using System.Collections.Generic;

namespace QSP.UI.Presenters.MiscInfo
{
    public class MiscInfoPresenter
    {
        public AirportMapPresenter MapPresenter { get; private set; }
        public DescentForcastPresenter ForcastPresenter { get; private set; }
        public MetarViewerPresenter MetarPresenter { get; private set; }

        public MiscInfoPresenter(
            IMiscInfoView view,
            AirportManager airportList,
            Locator<IWxTableCollection> windTableLocator,
            bool enableBrowser,
            Func<string> origGetter,
            Func<string> destGetter,
            Func<IEnumerable<string>> altnGetter)
        {
            MapPresenter = new AirportMapPresenter(view.AirportMapView, airportList);
            ForcastPresenter = new DescentForcastPresenter(view.ForcastView,
                airportList, windTableLocator, destGetter);
            MetarPresenter = new MetarViewerPresenter(view.MetarView, origGetter,
                destGetter, altnGetter);

            MapPresenter.BrowserEnabled = enableBrowser;

            Orig = origGetter();
            Dest = destGetter();
            Altn = altnGetter();
        }

        public AirportManager AirportList
        {
            get { return MapPresenter.Airports; }
            set
            {
                MapPresenter.Airports = value;
                ForcastPresenter.AirportList = value;
            }
        }

        public string Orig { set => MapPresenter.Orig = value; }
        public string Dest { set => MapPresenter.Dest = value; }
        public IEnumerable<string> Altn { set => MapPresenter.Altn = value; }
    }
}