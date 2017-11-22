using QSP.UI.Controllers;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.Presenters.FuelPlan.Route;
using QSP.UI.Util;
using QSP.UI.Views.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static CommonLibrary.AviationTools.Icao;

namespace QSP.UI.Views.FuelPlan.Route
{
    public partial class FinderOptionControl : UserControl, IFinderOptionView
    {
        private IMessageDisplay messageDisplay;
        private FinderOptionPresenter presenter;

        public FinderOptionControl()
        {
            InitializeComponent();
        }

        public void Init(FinderOptionPresenter presenter, IMessageDisplay messageDisplay)
        {
            filterBtn.Enabled = false;
            this.presenter = presenter;
            this.messageDisplay = messageDisplay;
        }

        public bool IsOrigin
        {
            set
            {
                fromIdentLbl.Text = value ? "Origin" : "Destination";
                sidLbl.Text = value ? "SID" : "STAR";
            }
        }

        public string Icao => TrimIcao(icaoTxtBox.Text);

        public IEnumerable<string> Runways
        {
            set => ComboBoxHelpers.SetItems(rwyComboBox, value);
        }

        public IEnumerable<string> Procedures
        {
            set
            {
                var items = procComboBox.Items;
                var val = value.ToArray();
                items.Clear();

                if (val.Length == 0)
                {
                    items.Add(FinderOptionPresenter.NoProcedureTxt);
                }
                else
                {
                    items.Add(FinderOptionPresenter.AutoProcedureTxt);
                    items.AddRange(val);
                }

                procComboBox.SelectedIndex = 0;
                filterBtn.Enabled = true;
            }
        }

        public string SelectedRwy { get =>rwyComboBox.Text; set =>rwyComboBox.Text=value; }

        public IEnumerable<string> SelectedProcedures => procComboBox.GetSelectedProcedures();

        public void ShowMessage(string msg, MessageLevel lvl)
        {
            messageDisplay.ShowMessage(msg, lvl);
        }

        public void ShowFilter(SidStarFileterPresenter p)
        {
            var filter = new SidStarFilterControl();
            filter.Init(p);
            filter.Location = new Point(0, 0);

            using (var frm = FormFactory.GetForm(filter.Size))
            {
                frm.Controls.Add(filter);

                filter.SelectionComplete += (_s, _e) =>
                {
                    frm.Close();

                    var selected = SelectedRwy;
                    IcaoChanged(this, EventArgs.Empty);
                    SelectedRwy = selected;
                };

                frm.ShowDialog();
            }
        }

        private void IcaoChanged(object sender, EventArgs e)
        {
            Runways = new string[0];
            Procedures = new string[0];
            filterBtn.Enabled = false;

            presenter.UpdateRunways();
        }

        private void origRwyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterBtn.Enabled = false;
            presenter.UpdateProcedures();
        }

        private void filterBtn_Click(object sender, EventArgs e)
        {
            presenter.ShowFilter(new SidStarFilterControl());
        }
    }
}
