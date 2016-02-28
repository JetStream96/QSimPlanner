using System.Windows.Forms;

namespace QSP.UI.Forms.LandingPerf
{
    public partial class CustomFuelControl : UserControl
    {
        private double zfwKg, fuelKg;

        public CustomFuelControl(double zfwKg,double fuelKg)
        {
            InitializeComponent();

            this.zfwKg = zfwKg;
            this.fuelKg = fuelKg;
        }
    }
}
