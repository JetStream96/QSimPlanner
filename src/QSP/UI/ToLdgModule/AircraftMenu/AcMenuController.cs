using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using System.Windows.Forms;
using QSP.AviationTools;
using static QSP.MathTools.Doubles;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public class AcMenuController
    {
        public static string NoToLdgProfileText = "None";

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
            var toItems = elem.ToProfile.Items;
            toItems.Clear();
            toItems.Add(NoToLdgProfileText);
            toItems.AddRange(profiles.TOTables
                .Select(t => t.Entry.ProfileName).ToArray());
            elem.ToProfile.SelectedIndex = 0;

            var ldgItems = elem.LdgProfile.Items;
            ldgItems.Clear();
            ldgItems.Add(NoToLdgProfileText);
            ldgItems.AddRange(profiles.LdgTables
                .Select(t => t.Entry.ProfileName).ToArray());
            elem.LdgProfile.SelectedIndex = 0;
        }

        private void initWtUnitCBox()
        {
            var units = new string[] { "KG", "LB" };

            var zfwUnit = elem.ZfwUnit;
            zfwUnit.Items.Clear();
            zfwUnit.Items.AddRange(units);
            zfwUnit.SelectedIndex = 0;

            var toUnit = elem.MaxToWtUnit;
            toUnit.Items.Clear();
            toUnit.Items.AddRange(units);
            toUnit.SelectedIndex = 0;

            var ldgUnit = elem.MaxLdgWtUnit;
            ldgUnit.Items.Clear();
            ldgUnit.Items.AddRange(units);
            ldgUnit.SelectedIndex = 0;

            wtUnitConnect();
        }

        private void wtUnitConnect()
        {
            elem.ZfwUnit.SelectedIndexChanged += wtUnitChanged;
            elem.MaxToWtUnit.SelectedIndexChanged += wtUnitChanged;
            elem.MaxLdgWtUnit.SelectedIndexChanged += wtUnitChanged;
        }

        private void wtUnitDisconnect()
        {
            elem.ZfwUnit.SelectedIndexChanged -= wtUnitChanged;
            elem.MaxToWtUnit.SelectedIndexChanged -= wtUnitChanged;
            elem.MaxLdgWtUnit.SelectedIndexChanged -= wtUnitChanged;
        }

        private void wtUnitChanged(object sender, EventArgs e)
        {
            var c = new ComboBox[] { elem.ZfwUnit,
                elem.MaxToWtUnit, elem.MaxToWtUnit };

            wtUnitDisconnect();

            int selIndex = ((ComboBox)sender).SelectedIndex;

            foreach (var i in c)
            {
                if (i != sender)
                {
                    i.SelectedIndex = selIndex;
                }
            }

            convertWeights(selIndex);

            wtUnitConnect();
        }

        private void convertWeights(int selIndex)
        {
            var factor = selIndex == 0 ?
                            Constants.LbKgRatio :
                            Constants.KgLbRatio;

            var textBoxes = new TextBox[] { elem.Zfw,
                elem.MaxToWt, elem.MaxLdgWt };

            foreach (var j in textBoxes)
            {
                double wt;

                if (double.TryParse(j.Text, out wt))
                {
                    j.Text = RoundToInt(wt * factor).ToString();
                }
            }
        }

        private void initAcType()
        {
            var acItems = elem.AcType.Items;
            acItems.Clear();

            acItems.AddRange(profiles.AcConfigs.Aircrafts.ToArray());
        }

        private void initListView()
        {
            var listItems = elem.AcListView.Items;
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
                .ElementAt(elem.AcListView.SelectedIndices[0]);

            fillProperties();
        }

        private void fillProperties()
        {
            var e = elem;
            var s = selectedConfig;

            e.AcType.Text = s.AC;
            e.Registration.Text = s.Registration;
            e.ToProfile.Text = s.TOProfile;
            e.LdgProfile.Text = s.LdgProfile;
            e.ZfwUnit.SelectedIndex = s.WtUnit == WeightUnit.KG ? 0 : 1;
            e.Zfw.Text = wtDisplay(s.ZfwKg);
            e.MaxToWt.Text = wtDisplay(s.MaxTOWtKg);
            e.MaxLdgWt.Text = wtDisplay(s.MaxLdgWtKg);
        }

        private string wtDisplay(double weightKg)
        {
            if (elem.ZfwUnit.SelectedIndex == 0)
            {
                // KG
                return RoundToInt(weightKg).ToString();
            }
            else
            {
                // LB
                return RoundToInt(weightKg * Constants.KgLbRatio).ToString();
            }
        }
    }
}
