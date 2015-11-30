using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QSP.AviationTools;
using QSP.RouteFinding;
using static QSP.UI.Utilities;

namespace QSP
{
    public partial class FindAltnForm
    {
        private void OK_Btn_Click(object sender, EventArgs e)
        {
            MainFormInstance().AltnTxtBox.Text = Convert.ToString(DataGrid[0, DataGrid.SelectedRows[0].Index].Value);
            this.Close();
        }

        private void AltnFinder_Load(object sender, EventArgs e)
        {
            Dest_Txtbox.Text = MainFormInstance().DestTxtBox.Text;
            m_ft.SelectedIndex = 0;
            MinRwyLength_Txtbox.Text = "2500";
        }


        private void Find_Btn_Click(object sender, EventArgs e)
        {
            int lengthFt = 0;
            //in feet
            if (m_ft.Text == "FT")
            {
                lengthFt = Convert.ToInt32(MinRwyLength_Txtbox.Text);
            }
            else
            {
                lengthFt = Convert.ToInt32(Convert.ToInt32(MinRwyLength_Txtbox.Text) * AviationConstants.M_FT_ratio);
            }

            var altn = AlternateFinder.AltnInfo(Dest_Txtbox.Text, lengthFt);

            DataGrid.Columns.Clear();
            DataGrid.Rows.Clear();
            DataGrid.ColumnCount = 4;
            DataGrid.RowCount = altn.Count;

            DataGrid.Columns[0].Name = "ICAO";
            DataGrid.Columns[1].Name = "Airport Name";
            if (m_ft.Text == "FT")
            {
                DataGrid.Columns[2].Name = "Max RWY Length(FT)";
            }
            else
            {
                DataGrid.Columns[2].Name = "Max RWY Length(M)";
            }

            DataGrid.Columns[3].Name = "Distance";

            for (int i = 0; i < altn.Count ; i++)
            {
                DataGrid[0, i].Value = altn[i].Icao ;
                DataGrid[1, i].Value = altn[i].AirportName;
                DataGrid[2, i].Value = altn[i].LongestRwyLength;
                DataGrid[3, i].Value = altn[i].Distance;
            }

            if (m_ft.Text == "M")
            {
                for (int i = 0; i < altn.Count; i++)
                {
                    DataGrid[2, i].Value = Math.Round(Convert.ToDouble(DataGrid[2, i].Value) / AviationConstants.M_FT_ratio);
                }
            }

            DataGrid.Sort(DataGrid.Columns[3], System.ComponentModel.ListSortDirection.Ascending);

            DataGrid.Columns[0].Width = 80;
            DataGrid.Columns[1].Width = 240;
            DataGrid.Columns[2].Width = 100;
            DataGrid.Columns[3].Width = 100;

            DataGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DataGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public FindAltnForm()
        {
            Load += AltnFinder_Load;
            InitializeComponent();
        }
    }
}
