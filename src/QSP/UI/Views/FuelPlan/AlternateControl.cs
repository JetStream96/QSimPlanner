using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.Util;
using QSP.UI.Controllers;
using QSP.LibraryExtension;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.RouteFinding.Tracks;
using QSP.WindAloft;

namespace QSP.UI.Views.FuelPlan
{
    public partial class AlternateControl : UserControl, IAlternateView
    {
        private Locator<AppOptions> appOptionsLocator;
        private AirwayNetwork airwayNetwork;
        private Locator<IWindTableCollection> windTableLocator;
        private DestinationSidSelection destSidProvider;
        private Func<FuelDataItem> fuelData;
        private Func<double> zfwTon;
        private Func<string> orig;
        private Func<string> dest;

        private AppOptions AppOptions => appOptionsLocator.Instance;

        public AlternateController AltnControl { get; private set; }

        public AlternateControl()
        {
            InitializeComponent();
        }

        public void Init(
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            Locator<IWindTableCollection> windTableLocator,
            DestinationSidSelection destSidProvider,
            Func<FuelDataItem> fuelData,
            Func<double> zfwTon,
            Func<string> orig,
            Func<string> dest)
        {
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.windTableLocator = windTableLocator;
            this.destSidProvider = destSidProvider;
            this.fuelData = fuelData;
            this.zfwTon = zfwTon;
            this.orig = orig;
            this.dest = dest;

            SetAltnController();
            AltnControl.RowCountChanged += 
                (s, e) => removeAltnBtn.Enabled = AltnControl.RowCount > 1;
        }

        public void SetBtnColorStyles(ControlDisableStyleController.ColorStyle style)
        {
            var removeBtnStyle = new ControlDisableStyleController(removeAltnBtn, style);
            removeBtnStyle.Activate();
        }

        public void SetAltnController()
        {
            AltnControl = new AlternateController(
                appOptionsLocator,
                airwayNetwork,
                altnLayoutPanel,
                destSidProvider,
                () => FuelPlanningControl.GetWindCalculator(AppOptions,
                    windTableLocator, airwayNetwork.AirportList, fuelData(),
                    zfwTon(), orig(), dest()));

            removeAltnBtn.Enabled = false;
            AltnControl.AddRow();
        }

        private void addAltnBtn_Click(object sender, EventArgs e)
        {
            AltnControl.AddRow();
        }

        private void removeAltnBtn_Click(object sender, EventArgs e)
        {
            AltnControl.RemoveLastRow();
        }
    }
}
