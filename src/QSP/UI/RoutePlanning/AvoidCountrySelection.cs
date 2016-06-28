using QSP.RouteFinding.Containers.CountryCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;
using QSP.UI.Controllers;

namespace QSP.UI.RoutePlanning
{
    public partial class AvoidCountrySelection : UserControl
    {
        private CountryCodeManager countryCodes;

        public AvoidCountrySelection()
        {
            InitializeComponent();
        }

        public IEnumerable<string> CheckedLetters
        {
            get
            {
                var items = listView.Items;
                
                return items.Cast<ListViewItem>()
                    .Where(i => i.Checked)
                    .Select(i => i.Text);
            }
        }

        public CountryCodeCollection CheckedCodes
        {
            get
            {
                var codes = CheckedLetters
                    .Select(s => countryCodes.GetCountryCode(s));

                return new CountryCodeCollection(codes);
            }
        }

        public void Init(CountryCodeManager countryCodes)
        {
            listView.CheckBoxes = true;
            this.countryCodes = countryCodes;
            AddItems();

            showSelectedCheckBox.CheckedChanged +=
                showSelectedCheckBoxChanged;

            new ListViewSortEnabler(listView).EnableSort();
        }

        private void AddItems()
        {
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

        private void showSelectedCheckBoxChanged(object sender, EventArgs e)
        {
            if (showSelectedCheckBox.Checked)
            {
                var selected = new HashSet<string>(CheckedLetters);
                listView.Items.Clear();

                var itemsToAdd = GetList()
                    .Where(p => selected.Contains(p.Key));

                foreach (var i in itemsToAdd)
                {
                    var lvi = new ListViewItem(i.Key);
                    lvi.SubItems.Add(i.Value);
                    listView.Items.Add(lvi);
                    lvi.Checked = true;
                }                
            }
            else
            {
                var selected = new HashSet<string>(CheckedLetters);
                listView.Items.Clear();
                AddItems();

                foreach (var i in listView.Items)
                {
                    var item = (ListViewItem)i;

                    if (selected.Contains(item.Text))
                    {
                        item.Checked = true;
                    }
                }
            }
        }
    }
}
