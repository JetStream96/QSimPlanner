using QSP.RouteFinding.Containers.CountryCode;
using QSP.UI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan.Route
{
    public partial class AvoidCountryView : UserControl
    {
        private CountryCodeManager countryCodes;
        private List<ListViewItem> items;
        public AvoidCountryView()
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

            set
            {
                foreach (var i in items)
                {
                    int code = countryCodes.GetCountryCode(i.Text);

                    if (value.Contains(code))
                    {
                        i.Checked = true;
                    }
                }
            }
        }

        public void Init(CountryCodeManager countryCodes)
        {
            listView.CheckBoxes = true;
            this.countryCodes = countryCodes;
            AddItems();

            showSelectedCheckBox.CheckedChanged +=
                ShowSelectedCheckBoxChanged;

            codeTxtBox.TextChanged += CodeTxtChanged;
            countryTxtBox.TextChanged += CountryTxtChanged;

            new ListViewSortEnabler(listView).EnableSort();
            ResizeListView();
        }

        private void ResizeListView()
        {
            listView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            int width = listView.Width -
                listView.Columns[0].Width -
                SystemInformation.VerticalScrollBarWidth - 4 ;

            listView.Columns[1].Width = Math.Max(0, width);
        }

        private void AddItems()
        {
            foreach (var i in GetList())
            {
                var lvi = new ListViewItem(i.Key);
                lvi.SubItems.Add(i.Value);
                listView.Items.Add(lvi);
            }

            items = listView.Items.Cast<ListViewItem>().ToList();
        }

        // KeyValuePair of LetterCode and CountryName
        private List<KeyValuePair<string, string>> GetList()
        {
            var list = countryCodes.FullNameLookup.ToList();
            list.Sort((x, y) => x.Key.CompareTo(y.Key));
            return list;
        }

        private void Filter(Func<ListViewItem, bool> predicate)
        {
            var selected = items.Where(predicate).ToArray();
            listView.Items.Clear();
            listView.Items.AddRange(selected);
        }

        private void ShowSelectedCheckBoxChanged(object sender, EventArgs e)
        {
            if (showSelectedCheckBox.Checked)
            {
                Filter(i => i.Checked);
            }
            else
            {
                Filter(i => true);
            }
        }

        private void CodeTxtChanged(object sender, EventArgs e)
        {
            var txt = codeTxtBox.Text.Trim().ToUpper();
            Filter(i => i.Text.IndexOf(txt) != -1);
        }

        private void CountryTxtChanged(object sender, EventArgs e)
        {
            var txt = countryTxtBox.Text.Trim().ToUpper();
            Filter(i => i.SubItems[1].Text.ToUpper().IndexOf(txt) != -1);
        }
    }
}
