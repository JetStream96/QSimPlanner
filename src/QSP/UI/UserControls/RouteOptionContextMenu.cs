using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.UI.RoutePlanning;
using System.Drawing;
using System.Windows.Forms;
using static QSP.UI.Views.Factories.FormFactory;

namespace QSP.UI.UserControls
{
    public class RouteOptionContextMenu : ContextMenuStrip
    {
        private Locator<CountryCodeCollection> checkedCodesLocator;
        private Locator<CountryCodeManager> countryCodesLocator;
        private ToolStripMenuItem avoidCountryStripMenuItem;
        
        public RouteOptionContextMenu(
            Locator<CountryCodeCollection> checkedCodesLocator,
            Locator<CountryCodeManager> countryCodesLocator) : base()
        {
            Init();

            this.checkedCodesLocator = checkedCodesLocator;
            this.countryCodesLocator = countryCodesLocator;
        }

        public void Subscribe()
        {
            avoidCountryStripMenuItem.Click += (s, e) =>
            {
                var countrySelectionView = new AvoidCountryView();
                countrySelectionView.Init(countryCodesLocator.Instance);
                countrySelectionView.Location = new Point(0, 0);
                countrySelectionView.CheckedCodes = checkedCodesLocator.Instance;

                using (var frm = GetForm(countrySelectionView.Size))
                {
                    frm.Controls.Add(countrySelectionView);

                    countrySelectionView.CancelBtn.Click += (_sender, _e) =>
                    {
                        frm.Close();
                    };

                    countrySelectionView.OkBtn.Click += (_sender, _e) =>
                    {
                        checkedCodesLocator.Instance =
                        countrySelectionView.CheckedCodes;

                        frm.Close();
                    };

                    frm.ShowDialog();
                }
            };
        }

        private void Init()
        {
            // 
            // avoidCountryStripMenuItem
            // 
            avoidCountryStripMenuItem = new ToolStripMenuItem();
            avoidCountryStripMenuItem.Name = "avoidCountryStripMenuItem";
            avoidCountryStripMenuItem.Size = new Size(181, 26);
            avoidCountryStripMenuItem.Text = "Avoided Countries";
            avoidCountryStripMenuItem.Visible = true;            
            // 
            // contextMenuStrip1
            // 
            ImageScalingSize = new Size(20, 20);
            Items.AddRange(new ToolStripItem[] {
            avoidCountryStripMenuItem});
            Name = "contextMenuStrip1";
            Size = new Size(182, 136);
        }
    }
}
