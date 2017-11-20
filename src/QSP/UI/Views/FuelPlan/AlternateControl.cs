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

        IReadOnlyList<IAlternateRowView> Views { get; }

        public AlternateControl()
        {
            InitializeComponent();
        }

        public void Init(AlternatePresenter presenter)
        {
            this.presenter = presenter;

            removeAltnBtn.Enabled = false;
            SetBtnColorStyles(ButtonColorStyle.Default);
            presenter.addrow
        }

        public IAlternateRowView AddRow()
        {
            var view = new AlternateRowControl();
            view.Init(,)
        }

        public void RemoveLastRow()
        {
            
        }


        private void SetBtnColorStyles(ControlDisableStyleController.ColorStyle style)
        {
            var removeBtnStyle = new ControlDisableStyleController(removeAltnBtn, style);
            removeBtnStyle.Activate();
        }
        
        private void addAltnBtn_Click(object sender, EventArgs e)
        { 

            AltnControl.AddRow();
        }

        private void removeAltnBtn_Click(object sender, EventArgs e)
        {
            AltnControl.RemoveLastRow();
            removeAltnBtn.Enabled = AltnControl.RowCount > 1;
        }
    }
}
