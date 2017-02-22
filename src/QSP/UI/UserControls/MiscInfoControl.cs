using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.UserControls.AirportMap;
using static QSP.MathTools.Numbers;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.UserControls
{
    public partial class MiscInfoControl : UserControl
    {
        private AirportMapControl airportMapControl = new AirportMapControl();
        private MetarViewer metarViewer = new MetarViewer();
        private DescentForcastDisplay desForcast = new DescentForcastDisplay();

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
                desForcast.AirportList = value;
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

            EnableTabControlAutosize();
            desForcast.Init(airportList, windTableLocator, destGetter);
            metarViewer.Init(origGetter, destGetter, altnGetter);
            TabControl1.SelectedIndex = 0;
        }

        public void SetOrig(string icao)
        {
            airportMapControl.Orig = icao;
        }

        public void SetDest(string icao)
        {
            airportMapControl.Dest = icao;
        }

        public void SetAltn(IEnumerable<string> icao)
        {
            airportMapControl.Altn = icao;
        }
        
        private void EnableTabControlAutosize()
        {
            Control[] controls = { airportMapControl, metarViewer };
            EventHandler adjustHeight = (s, e) =>
            {
                TabControl1.Height = GetHeight();
            };

            TabControl1.SelectedIndexChanged += adjustHeight;
            controls.ForEach(c => c.SizeChanged += adjustHeight);
        }

        private int GetHeight()
        {
            Control[] controls = { airportMapControl, metarViewer };
            const int minHeight = 800;
            int index = TabControl1.SelectedIndex;

            if (index == 2) return minHeight;
            return controls[index].Height + 100;
        }
    }
}
