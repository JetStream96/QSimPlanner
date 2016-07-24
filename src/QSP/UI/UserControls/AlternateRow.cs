using System.Windows.Forms;
using QSP.UI.Forms;
using QSP.RouteFinding.Airports;
using System;
using QSP.LibraryExtension;

namespace QSP.UI.UserControls
{
    public partial class AlternateRowItems : UserControl
    {
        private Func<string> destIcaoGetter;
        private Locator<AirportManager> airportListLocator;

        public AlternateRowItems()
        {
            InitializeComponent();
        }

        public void Init(Func<string> destIcaoGetter,
            Locator<AirportManager> airportListLocator)
        {
            this.destIcaoGetter = destIcaoGetter;
            this.airportListLocator = airportListLocator;
        }

        private void FindBtnClick(object sender, EventArgs e)
        {
            using (var frm = new FindAltnForm())
            {
                frm.AlternateSet += (_s, _e) =>
                {
                    IcaoTxtBox.Text = frm.SelectedIcao;
                };

                frm.Init(destIcaoGetter(), airportListLocator.Instance);
                frm.ShowDialog();
            }
        }
    }
}
