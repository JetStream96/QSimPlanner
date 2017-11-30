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
        private SidStarFilterPresenter presenter;
        private List<ListViewItem> items;

        public event EventHandler SelectionComplete;

        public bool IsBlacklist
        {
            get => listTypeComboBox.SelectedIndex == 0;
            set => listTypeComboBox.SelectedIndex = (value ? 0 : 1);
        }

        public IEnumerable<ProcedureEntry> SelectedProcedures()
        {
            return procListView.Items
                .Cast<ListViewItem>()
                .Where(lvi => lvi.Checked)
                .Select(lvi => new ProcedureEntry() { Name = lvi.Text, Ticked = lvi.Checked });
        }

        public SidStarFilterControl()
        {
            InitializeComponent();
        }

        public void Init(SidStarFilterPresenter presenter)
        {
            this.presenter = presenter;

            SetType();
            SetListView();
            InitAllProcedures(presenter.AllProcedures());

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

        private void InitAllProcedures((bool, IEnumerable<ProcedureEntry>) p)
        {
            var (isBlacklist, entries) = p;
            this.IsBlacklist = isBlacklist;
            items = entries.Select(i => new ListViewItem(i.Name) { Checked = i.Ticked }).ToList();
            procListView.Items.Clear();
            procListView.Items.AddRange(items.ToArray());
            procListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
