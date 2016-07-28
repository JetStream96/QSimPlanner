using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.UI.RoutePlanning;
using System.Drawing;
using System.Windows.Forms;
using static QSP.UI.Factories.FormFactory;

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
                var countrySelection = new AvoidCountrySelection();
                countrySelection.Init(countryCodesLocator.Instance);
                countrySelection.Location = new Point(0, 0);
                countrySelection.CheckedCodes = checkedCodesLocator.Instance;

                using (var frm = GetForm(countrySelection.Size))
                {
                    frm.Controls.Add(countrySelection);

                    countrySelection.CancelBtn.Click += (_sender, _e) =>
                    {
                        frm.Close();
                    };

                    countrySelection.OkBtn.Click += (_sender, _e) =>
                    {
                        checkedCodesLocator.Instance =
                        countrySelection.CheckedCodes;

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
