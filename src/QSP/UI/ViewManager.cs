using System.Windows.Forms;
using static QSP.UI.FormInstanceGetter;

namespace QSP.UI
{

    public class ViewManager
    {
        private MainForm frm;
        private PictureBox[] picBoxIcon;

        public ViewManager()
        {
            frm = MainFormInstance();

            picBoxIcon = new PictureBox[]{frm.PictureBox_Icon_Fuel,frm.PictureBox_Icon_TO,
                                          frm.PictureBox_Icon_LDG, frm.PictureBox_Icon_APData };
        }

        public enum Pages
        {
            FuelCalculation = 0,
            TakeoffPerf = 1,
            LandingPerf = 2,
            Misc = 3,
            MapOrigDest = 6,
            RouteTools = 7,
            FuelReport = 8
        }

        public void ShowPage(Pages pg)
        {
            frm.MainWin_TablessControl.SelectedIndex = (int)pg;
            RefreshPicBoxIcon(pg);
        }


        private void RefreshPicBoxIcon(Pages pg)
        {
            int page = (int)pg;
            if (page <= 3)
            {
                foreach (var i in picBoxIcon)
                {
                    i.Hide();
                }
                picBoxIcon[page].Show();
            }

        }

    }
}
