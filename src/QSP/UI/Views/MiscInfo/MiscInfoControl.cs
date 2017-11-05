using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.UI.Presenters.MiscInfo;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QSP.UI.Views.MiscInfo
{
    // TODO: Split the presenter.
    public partial class MiscInfoControl : UserControl
    {
        private AirportMapControl airportMapControl = new AirportMapControl();

        private MetarViewer metarViewer = new MetarViewer();
        private MetarViewerPresenter metarPresenter;

        private DescentForcastControl desForcast = new DescentForcastControl();
        private DescentForcastPresenter desPresenter;

        private Locator<IWindTableCollection> windTableLocator;
        private Func<string> destGetter;

        private AirportManager _airportList;
        public AirportManager AirportList
        {
            get { return _airportList; }

            set
            {
                _airportList = value;
                airportMapControl.Airports = value;
                desPresenter.AirportList = value;
            }
        }

        public MiscInfoControl()
        {
            InitializeComponent();
        }

        public void Init(
            AirportManager airportList,
            Locator<IWindTableCollection> windTableLocator,
            bool enableBrowser,
            Func<string> origGetter,
            Func<string> destGetter,
            Func<IEnumerable<string>> altnGetter)
        {
            this._airportList = airportList;
            airportMapControl.Init(airportList);
            this.windTableLocator = windTableLocator;
            airportMapControl.BrowserEnabled = enableBrowser;
            this.destGetter = destGetter;

            desPresenter = new DescentForcastPresenter(desForcast,
                airportList, windTableLocator, destGetter);
            desForcast.Init(desPresenter);

            metarPresenter = new MetarViewerPresenter(metarViewer,
                origGetter, destGetter, altnGetter);
            metarViewer.Init(metarPresenter);

            infoNavBar.Init(airportMapControl, metarViewer, desForcast, panel1);
        }

        public void SetOrig(string icao) => airportMapControl.Orig = icao;

        public void SetDest(string icao) => airportMapControl.Dest = icao;

        public void SetAltn(IEnumerable<string> icao) => airportMapControl.Altn = icao;
    }
}
