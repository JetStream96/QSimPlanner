using QSP.UI.Models;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Presenters.FuelPlan.Routes;
using QSP.UI.Util;
using QSP.UI.Views.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static CommonLibrary.AviationTools.Icao;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public partial class FinderOptionControl : UserControl, IFinderOptionView
    {
        public FinderOptionPresenter Presenter { get; private set; }

        public FinderOptionControl()
        {
            InitializeComponent();
        }

        public void Init(IFinderOptionModel model)
        {
            Presenter = new FinderOptionPresenter(this, model);
            filterBtn.Enabled = false;

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

        public string Icao { get => TrimIcao(icaoTxtBox.Text); set => icaoTxtBox.Text = value; }

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

        public IEnumerable<string> SelectedProcedures
        {
            get
            {
                var c = procComboBox;

                if (c.Text == FinderOptionPresenter.AutoProcedureTxt)
                {
                    return c.Items.Cast<string>()
                        .Where(s => s != FinderOptionPresenter.AutoProcedureTxt);
                }

                if (c.Text != FinderOptionPresenter.NoProcedureTxt) return new[] { c.Text };

                return new string[0];
            }

            set
            {
                var c = procComboBox;
                var val = value.ToList();

                if (val.Count > 1)
                {
                    c.Text = FinderOptionPresenter.AutoProcedureTxt;
                }
                else if (val.Count == 0)
                {
                    c.Text = FinderOptionPresenter.NoProcedureTxt;
                }
                else
                {
                    c.Text = val[0];
                }
            }
        }

        public string SelectedProcedureText
        {
            get => procComboBox.Text; set => procComboBox.Text = value;
        }

        public void ShowMessage(string msg, MessageLevel lvl)
        {
            ParentForm.ShowMessage(msg, lvl);
        }

        private void ShowFilter()
        {
            var filter = new SidStarFilterControl();
            var p = Presenter.GetFilterPresenter(filter);

            using (var frm = FormFactory.GetForm(filter.Size))
            {
                filter.Location = new Point(0, 0);
                frm.Controls.Add(filter);

                filter.Init(p);

                filter.SelectionComplete += (s, e) =>
                {
                    frm.Close();

                    var selected = SelectedRwy;
                    IcaoChanged(s, e);
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

            Presenter.UpdateRunways();
            Presenter.OnIcaoChanged(sender, e);
        }

        private void origRwyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterBtn.Enabled = false;
            Presenter.UpdateProcedures();
        }

        private void filterBtn_Click(object sender, EventArgs e)
        {
            ShowFilter();
        }
    }
}
