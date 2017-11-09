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
using QSP.RouteFinding.Tracks;
using QSP.WindAloft;

namespace QSP.UI.Views.FuelPlan
{
    public partial class AlternateControl : UserControl
    {
        private Locator<AppOptions> appOptionsLocator;
        private AirwayNetwork airwayNetwork;
        private Locator<IWindTableCollection> windTableLocator;

        public AlternateController AltnControl { get; private set; }

        public AlternateControl()
        {
            InitializeComponent();
        }

        public void Init(
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            Locator<IWindTableCollection> windTableLocator)
        {

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
                () => FuelPlanningControl.GetWindCalculator(appOptionsLocator.Instance, 
                    windTableLocator, ));

            removeAltnBtn.Enabled = false;
            AddAltn(this, EventArgs.Empty);
        }
    }
}
