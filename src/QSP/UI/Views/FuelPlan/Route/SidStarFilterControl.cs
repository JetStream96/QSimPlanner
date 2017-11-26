using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan.Route
{
    // Load the ProcedureFilter, and let user choose the settings.
    // Then ProcedureFilter is modified.
    //
    public partial class SidStarFilterControl : UserControl, ISidStarFilterView
    {
        private SidStarFileterPresenter presenter;

        public event EventHandler SelectionComplete;
        public bool IsBlacklist { get; set; }
        private List<ListViewItem> items;

        public IEnumerable<ProcedureEntry> SelectedProcedures =>
            procListView.Items
            .Cast<ListViewItem>()
            .Select(lvi => new ProcedureEntry() { Name = lvi.Text, Ticked = lvi.Checked });

        public SidStarFilterControl()
        {
            InitializeComponent();
        }

        public void Init(SidStarFileterPresenter presenter)
        {
            this.presenter = presenter;

            SetType();
            SetListView();
            presenter.InitView();

            showSelectedCheckBox.CheckedChanged += ShowSelectedCheckBoxChanged;

            okBtn.Click += UpdateFilter;
            cancelBtn.Click += (sender, e) => SelectionComplete?.Invoke(sender, e);

            new ListViewSortEnabler(procListView).EnableSort();
        }

        private void UpdateFilter(object sender, EventArgs e)
        {
            presenter.UpdateFilter();
            SelectionComplete?.Invoke(sender, e);
        }

        private void SetType()
        {
            listTypeComboBox.Items.Clear();
            listTypeComboBox.Items.AddRange(new[] { "Blacklist", "Whitelist" });

            listTypeComboBox.SelectedIndex = 0;
        }

        private void SetListView()
        {
            procListView.Columns.Add(new ColumnHeader());
            procListView.Columns[0].Text = presenter.IsSid ? "SID" : "STAR";
            procListView.CheckBoxes = true;
        }

        private void Filter(Func<ListViewItem, bool> predicate)
        {
            var selected = items.Cast<ListViewItem>().Where(predicate).ToArray();
            procListView.Items.Clear();
            procListView.Items.AddRange(selected);
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

        public void InitAllProcedures(IEnumerable<ProcedureEntry> e)
        {
            items = e.Select(i => new ListViewItem(i.Name) { Checked = i.Ticked }).ToList();
            procListView.Items.Clear();
            procListView.Items.AddRange(items.ToArray());
            procListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
