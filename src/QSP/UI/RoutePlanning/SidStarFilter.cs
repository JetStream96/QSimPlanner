using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.RoutePlanning
{
    // Load the ProcedureFilter, and let user choose the settings.
    // Then ProcedureFilter is modified.
    //
    public partial class SidStarFilter : UserControl
    {
        public event EventHandler FinishedSelection;

        private string icao;
        private string rwy;
        private List<string> procedures;
        private bool isSid;
        private ProcedureFilter procFilter;
        private List<ListViewItem> items;

        public SidStarFilter()
        {
            InitializeComponent();
        }

        public void Init(
            string icao,
            string rwy,
            List<string> procedures,
            bool isSid,
            ProcedureFilter procFilter)
        {
            this.icao = icao;
            this.rwy = rwy;
            this.procedures = procedures;
            this.isSid = isSid;
            this.procFilter = procFilter;

            SetType();
            SetListView();

            showSelectedCheckBox.CheckedChanged +=
                showSelectedCheckBoxChanged;

            okBtn.Click += UpdateFilter;
            cancelBtn.Click +=
                (sender, e) => FinishedSelection?.Invoke(sender, e);

            new ListViewSortEnabler(procListView).EnableSort();
        }

        private IEnumerable<string> CheckedItems()
        {
            bool isBlackList = listTypeComboBox.SelectedIndex == 0;

            return items
                .Where(i => i.Checked)
                .Select(i => i.Text);
        }

        private void UpdateFilter(object sender, EventArgs e)
        {
            procFilter[icao, rwy] = new FilterEntry(
                listTypeComboBox.SelectedIndex == 0,
                CheckedItems().ToList());

            FinishedSelection?.Invoke(sender, e);
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
            TickCheckBoxes();
        }

        private void TickCheckBoxes()
        {
            if (procFilter.Exists(icao, rwy))
            {
                var info = procFilter[icao, rwy];
                listTypeComboBox.SelectedIndex = info.IsBlackList ? 0 : 1;

                foreach (var i in items)
                {
                    i.Checked = info.Procedures.Contains(i.Text);
                }
            }
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
