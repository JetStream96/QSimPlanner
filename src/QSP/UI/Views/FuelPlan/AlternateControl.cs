using QSP.UI.Models;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan
{
    public partial class AlternateControl : UserControl, IAlternateView
    {
        private AlternatePresenter presenter;

        public IEnumerable<IAlternateRowView> Views =>
            Enumerable.Range(0, layoutPanel.RowCount)  
                      .Select(i => layoutPanel.GetControlFromPosition(0, i))
                      .Cast<IAlternateRowView>();

        public AlternateControl()
        {
            InitializeComponent();
        }

        public void Init(AlternatePresenter presenter)
        {
            this.presenter = presenter;

            removeAltnBtn.Enabled = false;
            SetBtnColorStyles(ButtonColorStyle.Default);
            presenter.AddRow();
            UpdateRemoveBtnEnabled();
        }

        /// <summary>
        /// Add a row with its presenter and initialize view of the row. 
        /// Returns the view of the created row.
        /// </summary>
        public IAlternateRowView AddRow()
        {
            SuspendLayout();

            var v = new AlternateRowControl();
            var p = presenter.GetRowPresenter(v);
            v.Init(p, ParentForm);
            v.AddToLayoutPanel(layoutPanel);

            ResumeLayout();
            return v;
        }

        private void SetBtnColorStyles(ControlDisableStyleController.ColorStyle style)
        {
            var removeBtnStyle = new ControlDisableStyleController(removeAltnBtn, style);
            removeBtnStyle.Activate();
        }

        private void addAltnBtn_Click(object sender, EventArgs e)
        {
            presenter.AddRow();
            UpdateRemoveBtnEnabled();
        }

        /// <exception cref="InvalidOperationException"></exception>
        private void removeAltnBtn_Click(object sender, EventArgs e)
        {
            presenter.RemoveLastRow();
            layoutPanel.Controls.Remove(
                layoutPanel.GetControlFromPosition(0, layoutPanel.RowCount - 1));
            layoutPanel.RowCount--;
            UpdateRemoveBtnEnabled();
        }

        private void UpdateRemoveBtnEnabled() => removeAltnBtn.Enabled = presenter.RowCount > 1;
    }
}
