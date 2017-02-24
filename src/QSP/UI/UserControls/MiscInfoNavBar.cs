using QSP.UI.Controllers.ControlGroup;
using QSP.UI.UserControls.AirportMap;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.UI.Controllers.ControlGroup.ControlSwitcher;
using static QSP.UI.Controllers.ControlGroup.GroupController;

namespace QSP.UI.UserControls
{
    public partial class MiscInfoNavBar : UserControl
    {
        private AirportMapControl airportMapControl;
        private MetarViewer metarViewer;
        private DescentForcastDisplay desForcast;

        private GroupController btnControl;
        private ControlSwitcher viewControl;

        private Panel panel;

        private IEnumerable<Control> AllPages => new Control[]
        {
            airportMapControl, metarViewer, desForcast
        };

        public MiscInfoNavBar()
        {
            InitializeComponent();
        }

        public void Init(AirportMapControl airportMapControl,
            MetarViewer metarViewer, DescentForcastDisplay desForcast, Panel panel)
        {
            this.airportMapControl = airportMapControl;
            this.metarViewer = metarViewer;
            this.desForcast = desForcast;
            this.panel = panel;

            SetControlPosition();
            EnableControlColors();
            EnableViewControl();
        }

        private void SetControlPosition()
        {
            foreach (var i in AllPages)
            {
                i.Location = Point.Empty;
                i.Visible = i == airportMapControl;
                panel.Controls.Add(i);
            }
        }

        private void EnableViewControl()
        {
            viewControl = new ControlSwitcher(
                new ControlPair(lbl1, airportMapControl),
                new ControlPair(lbl2, metarViewer),
                new ControlPair(lbl3, desForcast));

            viewControl.Subscribed = true;
        }

        private ColorGroup ColorStyle => new ColorGroup(
            Color.Black, Color.White,
            Color.White, Color.DimGray,
            Color.FromArgb(0, 174, 219), Color.White);

        private void EnableControlColors()
        {
            var pairs = new[] { lbl1, lbl2, lbl3 }
                .Select(p => new ControlColorPair(p, ColorStyle));

            btnControl = new GroupController(pairs.ToArray());
            btnControl.Initialize();
            btnControl.SetSelected(lbl1);
        }
    }
}
