using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public class AcMenuController
    {
        private AcMenuElements elem;
        private ProfileManager profiles;
        private AircraftConfig selectedConfig;

        public AcMenuController(AcMenuElements elem, ProfileManager profiles)
        {
            this.elem = elem;
            this.profiles = profiles;
        }

        public void InitializeControls()
        {
            initListView();
            initAcType();
            initToLdgCBox();
            initWtUnitCBox();
        }

        private void initToLdgCBox()
        {
            var toItems = elem.toProfile.Items;
            toItems.Clear();
            toItems.AddRange(profiles.TOTables
                .Select(t => t.Entry.ProfileName).ToArray());

            var ldgItems = elem.ldgProfile.Items;
            ldgItems.Clear();
            ldgItems.AddRange(profiles.LdgTables
                .Select(t => t.Entry.ProfileName).ToArray());
        }

        private void initWtUnitCBox()
        {
            var units = new string[] { "KG", "LB" };

            var zfwUnit = elem.zfwUnit;
            zfwUnit.Items.Clear();
            zfwUnit.Items.AddRange(units);
            zfwUnit.SelectedIndex = 0;

            var toUnit = elem.maxToWtUnit;
            toUnit.Items.Clear();
            toUnit.Items.AddRange(units);
            toUnit.SelectedIndex = 0;

            var ldgUnit = elem.maxLdgWtUnit;
            ldgUnit.Items.Clear();
            ldgUnit.Items.AddRange(units);
            ldgUnit.SelectedIndex = 0;
        }

        private void initAcType()
        {
            var acItems = elem.acType.Items;
            acItems.Clear();

            acItems.AddRange(profiles.AcConfigs.Aircrafts.ToArray());
        }

        private void initListView()
        {
            var listItems = elem.acListView.Items;
            listItems.Clear();

            foreach (var i in profiles.AcConfigs.Aircrafts)
            {
                var lvi = new ListViewItem(i.AC);
                lvi.SubItems.Add(i.Registration);
                // lvi.ImageIndex = (int)i.Severity;
                listItems.Add(lvi);
            }
        }

        public void SelectedAcChanged(object sender, EventArgs e)
        {
            selectedConfig = profiles.AcConfigs.Aircrafts
                .ElementAt(elem.acListView.SelectedIndices[0]);

            fillProperties();
        }

        private void fillProperties()
        {
            elem.acType.Text = selectedConfig.AC;
            elem.registration.Text = selectedConfig.Registration;
            elem.toProfile.Text = selectedConfig.TOProfile;
            // TODO: what if the one does not exist?
            // Maybe add 'None'.
        }        
    }
}
