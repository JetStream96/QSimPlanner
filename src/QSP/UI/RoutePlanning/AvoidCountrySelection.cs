using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.RouteFinding.Containers.CountryCode;

namespace QSP.UI.RoutePlanning
{
    public partial class AvoidCountrySelection : UserControl
    {
        private CountryCodeManager countryCodes;

        public AvoidCountrySelection()
        {
            InitializeComponent();
        }

        public void Init(CountryCodeManager countryCodes)
        {
            listView.CheckBoxes = true;
            this.countryCodes = countryCodes;

            foreach (var i in GetList())
            {
                var lvi = new ListViewItem(i.Key);
                lvi.SubItems.Add(i.Value);
                listView.Items.Add(lvi);
            }
        }

        // KeyValuePair of LetterCode and CountryName
        private List<KeyValuePair<string, string>> GetList()
        {
            var list = countryCodes.FullNameLookup.ToList();
            list.Sort((x, y) => x.Key.CompareTo(y.Key));
            return list;
        }
    }
}
