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
        private List<ListViewItem> items;

        public event EventHandler SelectionComplete;
        public bool IsBlacklist { get; set; }

        public IEnumerable<ProcedureEntry> Procedures
        {
            get => procListView.Items
                .Cast<ListViewItem>()
                .Select(lvi => new ProcedureEntry() {Name = lvi.Text, Ticked = lvi.Checked});

            set
            {
                var items = procListView.Items;
                items.Clear();

                foreach (var i in value)
                {
                    var lvi = new ListViewItem(i.Name) { Checked = i.Ticked };
                    items.Add(lvi);
                }

                this.items = items.Cast<ListViewItem>().ToList();
                procListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

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
            procListView.Columns[0].Text = presenter.IsSid ? "SID" : "STAR";
            procListView.CheckBoxes = true;
        }
        
        private void Filter(Func<ListViewItem, bool> predicate)
        {
            var selected = items.Where(predicate).ToArray();
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
    }
}
