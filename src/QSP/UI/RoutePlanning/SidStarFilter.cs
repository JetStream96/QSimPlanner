using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.Controllers;

namespace QSP.UI.RoutePlanning
{
    public partial class SidStarFilter : UserControl
    {
        public event EventHandler FinishedSelection;

        private List<string> procedures;
        private bool isSid;
        private List<ListViewItem> items;

        public SidStarFilter()
        {
            InitializeComponent();
        }

        public IEnumerable<string> Selected
        {
            get
            {
                bool isBlackList = listTypeComboBox.SelectedIndex == 0;

                return items
                    .Where(i => i.Checked ^ isBlackList)
                    .Select(i => i.Text);
            }
        }

        public void Init(List<string> procedures, bool isSid)
        {
            this.procedures = procedures;
            this.isSid = isSid;

            SetType();
            SetListView();

            showSelectedCheckBox.CheckedChanged +=
                showSelectedCheckBoxChanged;

            okBtn.Click += FinishedSelection;
            cancelBtn.Click += FinishedSelection;

            new ListViewSortEnabler(procListView).EnableSort();
        }

        private void SetType()
        {
            listTypeComboBox.Items.Clear();
            listTypeComboBox.Items.AddRange(new string[]
                {"Blacklist", "Whitelist"});

            listTypeComboBox.SelectedIndex = 0;
        }

        private void SetListView()
        {
            procListView.Columns[0].Text = isSid ? "SID" : "STAR";
            procListView.CheckBoxes = true;

            var items = procListView.Items;

            items.Clear();

            foreach (var i in procedures)
            {
                items.Add(new ListViewItem(i));
            }

            procListView.Columns[0].Width = procListView.Width - 30;

            this.items = items.Cast<ListViewItem>().ToList();
        }

        private void filter(Func<ListViewItem, bool> predicate)
        {
            var selected = items.Where(predicate).ToArray();
            procListView.Items.Clear();
            procListView.Items.AddRange(selected);
        }

        private void showSelectedCheckBoxChanged(object sender, EventArgs e)
        {
            if (showSelectedCheckBox.Checked)
            {
                filter(i => i.Checked);
            }
            else
            {
                filter(i => true);
            }
        }
    }
}
