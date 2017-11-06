using QSP.UI.Presenters.MiscInfo;
using System.Windows.Forms;

namespace QSP.UI.Views.MiscInfo
{
    public partial class MiscInfoControl : UserControl, IMiscInfoView
    {
        private AirportMapControl map = new AirportMapControl();
        private DescentForcastControl forcast = new DescentForcastControl();
        private MetarViewer metar = new MetarViewer();

        public IAirportMapView AirportMapView => map;
        public IDescentForcastView ForcastView => forcast;
        public IMetarViewerView MetarView => metar;

        public MiscInfoControl()
        {
            InitializeComponent();
        }

        public void Init(MiscInfoPresenter p)
        {
            map.Init(p.MapPresenter);
            forcast.Init(p.ForcastPresenter);
            metar.Init(p.MetarPresenter);

            infoNavBar.Init(map, metar, forcast, panel1);
        }
    }
}
