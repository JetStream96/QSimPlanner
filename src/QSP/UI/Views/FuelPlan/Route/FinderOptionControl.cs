using QSP.UI.Models;
using QSP.UI.Models.FuelPlan;
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

            SetButtonColorStyle();
        }

        private void SetButtonColorStyle()
        {
            var style = ButtonColorStyle.Default;
            var c = new ControlDisableStyleController(filterBtn, style);
            c.Activate();
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

        public string SelectedRwy { get => rwyComboBox.Text; set => rwyComboBox.Text = value; }

        public SelectedProcedures SelectedProcedures
        {
            get
            {
                var c = procComboBox;

                if (c.Text == FinderOptionPresenter.AutoProcedureTxt)
                {
                    return SelectedProcedures.Auto(c.Items.Cast<string>()
                        .Where(s => s != FinderOptionPresenter.AutoProcedureTxt).ToList());
                }

                if (c.Text != FinderOptionPresenter.NoProcedureTxt)
                {
                    return SelectedProcedures.Selected(c.Text);
                }

                return SelectedProcedures.None;
            }

            set
            {
                var c = procComboBox;

                if (value.IsAuto)
                {
                    c.Text = FinderOptionPresenter.AutoProcedureTxt;
                }
                else if (value.IsNone)
                {
                    c.Text = FinderOptionPresenter.NoProcedureTxt;
                }
                else if (value.Strings.Count == 1)
                {
                    c.Text = value.Strings[0];
                }
            }
        }

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
            presenter.OnIcaoChanged(sender, e);
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
