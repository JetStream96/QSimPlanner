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

        public IEnumerable<IAlternateRowView> Subviews =>
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
            AddRow();
        }

        /// <summary>
        /// Add a row with its presenter and initialize the view. 
        /// </summary>
        public void AddRow()
        {
            var v = new AlternateRowControl();
            var p = presenter.GetRowPresenter(v);
            v.TabIndex = 0;
            v.Init(p);
            v.AddToLayoutPanel(layoutPanel);
            presenter.SubsribeRowEventHandlers(v);
            UpdateRemoveBtnEnabled();
        }

        private void SetBtnColorStyles(ControlDisableStyleController.ColorStyle style)
        {
            var removeBtnStyle = new ControlDisableStyleController(removeAltnBtn, style);
            removeBtnStyle.Activate();
        }

        private void addAltnBtn_Click(object sender, EventArgs e)
        {
            ParentForm.SuspendDrawingWhen(() => AddRow());
        }

        /// <exception cref="InvalidOperationException"></exception>
        private void removeAltnBtn_Click(object sender, EventArgs e)
        {
            Parent.SuspendDrawingWhen(() =>
            {
                presenter.RemoveLastRow();

                var row = layoutPanel.GetControlFromPosition(0, layoutPanel.RowCount - 1);
                layoutPanel.Controls.Remove(row);
                layoutPanel.RowCount--;
                row.Dispose();
                UpdateRemoveBtnEnabled();
            });
        }

        private void UpdateRemoveBtnEnabled() => removeAltnBtn.Enabled = presenter.RowCount > 1;
    }
}
