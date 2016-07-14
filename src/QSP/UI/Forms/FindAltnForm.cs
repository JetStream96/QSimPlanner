using QSP.RouteFinding.Airports;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using static QSP.AviationTools.Constants;
using static QSP.UI.FormInstanceGetter;

namespace QSP
{
    public partial class FindAltnForm
    {
        private AirportManager airportList;
        
        public FindAltnForm()
        {
            InitializeComponent();
        }

        public void Init(AirportManager airportList)
        {
            this.airportList = airportList;
            lengthUnitComboBox.SelectedIndex = 0;
            Load += AltnFinder_Load;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            MainFormInstance().AltnTxtBox.Text =
                DataGrid[0, DataGrid.SelectedRows[0].Index].Value.ToString();

            Close();
        }

        private void AltnFinder_Load(object sender, EventArgs e)
        {
            icaoTxtbox.Text = MainFormInstance().DestTxtBox.Text;
            lengthUnitComboBox.SelectedIndex = 0;
            minRwyLengthTxtbox.Text = "2500";
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

        private void FindBtn_Click(object sender, EventArgs e)
        {
            double lengthFt;

            if (TryGetLength(out lengthFt) == false)
            {
                MessageBox.Show("Invalid runway length.");
                return;
            }

            var altn = new AlternateFinder(airportList)
                .AltnInfo(icaoTxtbox.Text, (int)lengthFt);

            DataGrid.Columns.Clear();
            DataGrid.Rows.Clear();
            DataGrid.ColumnCount = 4;
            DataGrid.RowCount = altn.Count;

            DataGrid.Columns[0].Name = "ICAO";
            DataGrid.Columns[1].Name = "Airport Name";
            DataGrid.Columns[2].Name = "Max RWY Length(" +
                                       lengthUnitComboBox.Text +
                                       ")";

            DataGrid.Columns[3].Name = "Distance";

            for (int i = 0; i < altn.Count; i++)
            {
                DataGrid[0, i].Value = altn[i].Icao;
                DataGrid[1, i].Value = altn[i].AirportName;
                DataGrid[2, i].Value = altn[i].LongestRwyLength;
                DataGrid[3, i].Value = altn[i].Distance;
            }

            if (lengthUnitComboBox.SelectedIndex == 0)
            {
                for (int i = 0; i < altn.Count; i++)
                {
                    DataGrid[2, i].Value = (int)Math.Round(
                        Convert.ToDouble(DataGrid[2, i].Value) * FtMeterRatio);
                }
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

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
