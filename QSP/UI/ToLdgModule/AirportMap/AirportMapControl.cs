using QSP.Metar;
using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;

namespace QSP.UI.ToLdgModule.AirportMap
{
    public partial class AirportMapControl : UserControl
    {
        public AirportManager AirportList { get; set; }

        private string CurrentIcao
        {
            get
            {
                return icaoComboBox.Text.Trim().ToUpper();
            }
        }

        private WebBrowser browser { get; set; }

        public AirportMapControl()
        {
            InitializeComponent();
        }

        public void InitializeControls()
        {
            resetAirport();
            setEmptyDataGrid();

            icaoComboBox.Items.Clear();
            icaoComboBox.Text = "";
        }

        private void setEmptyDataGrid()
        {
            airportDataGrid.Columns.Clear();
            airportDataGrid.Rows.Clear();
            airportDataGrid.ColumnCount = 10;
            airportDataGrid.RowCount = 0;

            setColumnsLables();
        }

        private void resetAirport()
        {
            airportNameLbl.Text = "";
            latLonLbl.Text = "";
            elevationLbl.Text = "";
            transAltLbl.Text = "";
            metarLbl.Text = "";
            metarLbl.Visible = false;
            updateBtn.Visible = false;
        }

        private async void setMetar(string icao)
        {
            metarLbl.Text = "Updating ...";
            metarLbl.Text = await Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetar(icao));
            metarLbl.Visible = true;
            updateBtn.Visible = true;
        }

        private void setAirport(Airport airport)
        {
            setMetar(airport.Icao);

            airportNameLbl.Text = airport.Name;
            latLonLbl.Text = airport.Lat + " / " + airport.Lon;
            elevationLbl.Text = airport.Elevation + " FT";
            transAltLbl.Text = transitionAlts(airport);
        }

        private string transitionAlts(Airport airport)
        {
            // If TL is 0, that means it's not a fixed value.
            // Show "-" instead.
            if (airport.TransLvl == 0)
            {
                return airport.TransAlt.ToString() + " / -";
            }
            else
            {
                return airport.TransAlt.ToString() + " / FL" +
                    RoundToInt(airport.TransLvl / 100.0).ToString();
            }
        }

        private string surfaceType(int type)
        {
            switch (type)
            {
                case 0:
                    return "Concrete";

                case 1:
                    return "Asphalt or Bitumen";

                case 2:
                    return "Gravel, Coral Or Ice";

                case 3:
                    return "Other";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void updateDataGrid(Airport airport)
        {
            airportDataGrid.Columns.Clear();
            airportDataGrid.Rows.Clear();
            airportDataGrid.ColumnCount = 10;
            airportDataGrid.RowCount = airport.Rwys.Count;
            setColumnsLables();

            for (int i = 0; i < airport.Rwys.Count; i++)
            {
                var rwy = airport.Rwys[i];

                airportDataGrid[0, i].Value = rwy.RwyIdent;
                airportDataGrid[1, i].Value = rwy.Length;
                airportDataGrid[2, i].Value = rwy.Heading;
                airportDataGrid[3, i].Value = rwy.Lat;
                airportDataGrid[4, i].Value = rwy.Lon;

                if (rwy.IlsAvail)
                {
                    airportDataGrid[5, i].Value = rwy.IlsFreq;
                    airportDataGrid[6, i].Value = rwy.IlsHeading;
                }
                else
                {
                    airportDataGrid[5, i].Value = "";
                    airportDataGrid[6, i].Value = "";
                }

                airportDataGrid[7, i].Value = rwy.ThresholdOverflyHeight;
                airportDataGrid[8, i].Value = rwy.GlideslopeAngle.ToString("0.0");
                airportDataGrid[9, i].Value = surfaceType(rwy.SurfaceType);
            }
        }


        private void setColumnsLables()
        {
            airportDataGrid.Columns[0].Name = "RWY";
            airportDataGrid.Columns[1].Name = "Length(FT)";
            airportDataGrid.Columns[2].Name = "Heading";
            airportDataGrid.Columns[3].Name = "LAT";
            airportDataGrid.Columns[4].Name = "LON";
            airportDataGrid.Columns[5].Name = "ILS freq";
            airportDataGrid.Columns[6].Name = "ILS course";
            airportDataGrid.Columns[7].Name = "Threshold altitude(FT)";
            airportDataGrid.Columns[8].Name = "Glideslope angle";
            airportDataGrid.Columns[9].Name = "Surface Type";
        }

        private void findAirport()
        {
            resetAirport();
            setEmptyDataGrid();

            var airport = AirportList.Find(CurrentIcao);

            if (airport != null && airport.Rwys.Count > 0)
            {
                setAirport(airport);
                updateDataGrid(airport);
                ShowMap(airport.Lat, airport.Lon);
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            setMetar(CurrentIcao);
        }

        private void icaoComboBox_TextChanged(object sender, EventArgs e)
        {
            findAirport();
        }

        private void metarLbl_Click(object sender, EventArgs e)
        {
            var frm = new MetarForm();
            frm.icaoTxtBox.Text = CurrentIcao;
            frm.icaoTxtBox.Enabled = false;
            frm.resultRichTxtBox.Text = metarLbl.Text ?? "";
            frm.sendBtn.Visible = false;
            frm.downloadBtn.Visible = false;
            frm.getTafCheckBox.Visible = false;
            frm.ShowDialog();
        }

        public void EnableBrowser()
        {
            var wb = new WebBrowser();
            
            wb.Location = new Point(-3, 270);
            wb.Size = new Size(1021, 384);
            
            Controls.Add(wb);
            browser = wb;
        }

        public void DisableBrowser()
        {
            Controls.Remove(browser);
            browser = null;
        }

        public void ShowMap(double lat, double lon)
        {
            if (browser != null)
            {
                showMapBrowser(lat, lon);
            }
        }

        private void showMapBrowser(double lat, double lon)
        {
            // This requires a registry fix. (IE emulation)

            browser.DocumentText = GoogleMapGenerator.GetHtml(
                lat, lon, browser.Width, browser.Height);
        }
    }
}
