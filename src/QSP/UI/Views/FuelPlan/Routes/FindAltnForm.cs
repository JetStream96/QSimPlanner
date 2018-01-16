using QSP.MathTools;
using QSP.RouteFinding.Airports;
using QSP.UI.Presenters;
using QSP.UI.Util;
using QSP.Utilities.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using static QSP.AviationTools.Constants;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public partial class FindAltnForm : Form, IFindAltnView
    {
        public event EventHandler AlternateSet;

        private FindAltnPresenter presenter;

        public FindAltnForm()
        {
            InitializeComponent();
        }

        public void Init(string icao, FindAltnPresenter presenter)
        {
            icaoTxtbox.Text = icao;
            this.presenter = presenter;
            lengthUnitComboBox.SelectedIndex = (int)LengthUnit.Meter;
            minRwyLengthTxtbox.Text = "2500";
            presenter.FindAlternates();
        }

        public string SelectedIcao
        {
            get
            {
                var selected = DataGrid.SelectedRows;

                if (selected.Count == 0)
                {
                    return "";
                }

                return (string)DataGrid[0, selected[0].Index].Value;
            }
        }

        public string ICAO => icaoTxtbox.Text;

        public string Length => minRwyLengthTxtbox.Text;

        public LengthUnit LengthUnit => (LengthUnit)lengthUnitComboBox.SelectedIndex;

        public IReadOnlyList<AlternateFinder.AlternateInfo> Alternates
        {
            set
            {
                DataGrid.Columns.Clear();
                DataGrid.Rows.Clear();
                DataGrid.ColumnCount = 4;
                DataGrid.RowCount = value.Count;

                DataGrid.Columns[0].Name = "ICAO";
                DataGrid.Columns[1].Name = "Airport Name";
                DataGrid.Columns[2].Name = "Max RWY Length(" + lengthUnitComboBox.Text + ")";

                DataGrid.Columns[3].Name = "Distance";

                for (int i = 0; i < value.Count; i++)
                {
                    DataGrid[0, i].Value = value[i].Icao;
                    DataGrid[1, i].Value = value[i].AirportName;
                    DataGrid[2, i].Value = Numbers.RoundToInt(value[i].LongestRwyLengthFt *
                        (LengthUnit == LengthUnit.Meter ? FtMeterRatio : 1));
                    DataGrid[3, i].Value = value[i].Distance;
                }

                DataGrid.Sort(DataGrid.Columns[3], ListSortDirection.Ascending);

                DataGrid.Columns[0].Width = 80;
                DataGrid.Columns[1].Width = 240;
                DataGrid.Columns[2].Width = 100;
                DataGrid.Columns[3].Width = 100;

                DataGrid.Columns[0].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                DataGrid.Columns[2].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

                DataGrid.Columns[3].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }
        }

        public void ShowWarning(string msg) => MsgBoxHelper.ShowWarning(this, msg);

        private void OkBtn_Click(object sender, EventArgs e)
        {
            AlternateSet?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private bool TryGetLength(out double lengthFt)
        {
            if (double.TryParse(minRwyLengthTxtbox.Text, out lengthFt))
            {
                if (lengthUnitComboBox.SelectedIndex == 0)
                {
                    lengthFt *= MeterFtRatio;
                }

                return true;
            }

            return false;
        }

        private void FindBtnClick(object sender, EventArgs e)
        {
            presenter.FindAlternates();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
