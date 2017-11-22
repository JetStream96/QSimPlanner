using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.Util;
using QSP.UI.Controllers;
using QSP.LibraryExtension;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.WindAloft;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Models;

namespace QSP.UI.Views.FuelPlan
{
    public partial class AlternateControl : UserControl, IAlternateView
    {
        private AlternatePresenter presenter;

        public IEnumerable<IAlternateRowView> Views => 
            layoutPanel.Controls.Cast<IAlternateRowView>();

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
        }

        /// <summary>
        /// Add a row with its presenter and initialize view of the row. 
        /// Returns the view of the created row.
        /// </summary>
        public IAlternateRowView AddRow()
        {
            var v = new AlternateRowControl();
            var p = presenter.GetRowPresenter(v);
            v.Init(p, ParentForm);
            v.AddToLayoutPanel(layoutPanel);
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
        }
        
        /// <exception cref="InvalidOperationException"></exception>
        private void removeAltnBtn_Click(object sender, EventArgs e)
        {
            presenter.RemoveLastRow();
            layoutPanel.RowCount--;
            removeAltnBtn.Enabled = presenter.RowCount > 1;
        }
    }
}
