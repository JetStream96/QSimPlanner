using QSP.UI.Presenters.MiscInfo;
using System.Windows.Forms;

namespace QSP.UI.Views.MiscInfo
{
    public partial class DescentForcastControl : UserControl, IDescentForcastView
    {
        private DescentForcastPresenter presenter;

        public string LastUpdateTime
        {
            set { lastUpdatedLbl.Text = $"Last Updated : {value}"; }
        }

        public string DestinationIcao
        {
            set { destIcaoLbl.Text = "Destination : " + value; }
        }

        public string Forcast
        {
            set { desForcastRichTxtBox.Text = value; }

        }

        public DescentForcastControl()
        {
            InitializeComponent();
        }

        public void Init(DescentForcastPresenter presenter)
        {
            this.presenter = presenter;
            destIcaoLbl.Text = "";
            lastUpdatedLbl.Text = "";

            updateDesForcastBtn.Click += (s, e) => presenter.UpdateForcast();
        }

        private void DescentForcastControl_Load(object sender, System.EventArgs e)
        {
            presenter.UpdateForcast();
        }
    }
}
