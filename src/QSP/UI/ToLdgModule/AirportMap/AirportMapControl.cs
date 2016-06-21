using QSP.GoogleMap;
using QSP.Metar;
using QSP.RouteFinding.Airports;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;

namespace QSP.UI.ToLdgModule.AirportMap
{
    public partial class AirportMapControl : UserControl
    {
        public AirportManager Airports { get; set; }

        private string CurrentIcao
        {
            get
            {
                return icaoComboBox.Text.Trim().ToUpper();
            }
        }

        public bool BrowserEnabled
        {
            get
            {
                return browser != null;
            }

            set
            {
                if (value && BrowserEnabled == false)
                {
                    EnableBrowser();
                }
                else if (value == false && BrowserEnabled)
                {
                    DisableBrowser();
                }
            }
        }

        public bool StaticMapEnabled
        {
            get
            {
                return picBox != null;
            }

            set
            {
                if (value && StaticMapEnabled == false)
                {
                    EnableStaticMap();
                }
                else if (value == false && StaticMapEnabled)
                {
                    DisableStaticMap();
                }
            }
        }

        private string _orig;

        public string Orig
        {
            get
            {
                return _orig;
            }

            set
            {
                _orig = value;
                SetIcaoItems();
            }
        }

        private string _dest;

        public string Dest
        {
            get
            {
                return _dest;
            }

            set
            {
                _dest = value;
                SetIcaoItems();
            }
        }

        private string _altn;

        public string Altn
        {
            get
            {
                return _altn;
            }

            set
            {
                _altn = value;
                SetIcaoItems();
            }
        }

        private void SetIcaoItems()
        {
            icaoComboBox.Items.Clear();

            icaoComboBox.Items.AddRange(
                new string[] { _orig, _dest, _altn }
                .Where(s => string.IsNullOrEmpty(s) == false)
                .ToArray());
        }

        private PictureBox picBox;
        private WebBrowser browser;

        public AirportMapControl()
        {
            InitializeComponent();
        }

        public void Initialize(AirportManager airports)
        {
            ResetAirport();
            SetEmptyDataGrid();

            icaoComboBox.Text = "";
            this.Airports = airports;

            AddToolTip();
        }

        private void AddToolTip()
        {
            var tp = new ToolTip();

            tp.AutoPopDelay = 5000;
            tp.InitialDelay = 1000;
            tp.ReshowDelay = 500;
            tp.ShowAlways = true;

            tp.SetToolTip(metarLbl, "View METAR");
            tp.SetToolTip(updateBtn, "Refresh METAR");
        }

        private void SetEmptyDataGrid()
        {
            airportDataGrid.Columns.Clear();
            airportDataGrid.Rows.Clear();
            airportDataGrid.ColumnCount = 10;
            airportDataGrid.RowCount = 0;

            SetColumnsLables();
        }

        private void ResetAirport()
        {
            airportNameLbl.Text = "";
            latLonLbl.Text = "";
            elevationLbl.Text = "";
            transAltLbl.Text = "";
            metarLbl.Text = "";
            metarLbl.Visible = false;
            updateBtn.Visible = false;
        }

        // TODO: Need the ability to cancel the task.
        private async void SetMetar(string icao)
        {
            metarLbl.Text = "Updating ...";
            metarLbl.Text = await Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetar(icao));
            metarLbl.Visible = true;
            updateBtn.Visible = true;
        }

        private void SetAirport(Airport airport)
        {
            SetMetar(airport.Icao);

            airportNameLbl.Text = airport.Name;
            latLonLbl.Text = airport.Lat.ToString("#.######") + " / " +
                airport.Lon.ToString("#.######");
            elevationLbl.Text = airport.Elevation + " FT";

            if (airport.TransAvail)
            {
                transExistLbl.Visible = true;
                transAltLbl.Text = TransitionAlts(airport);
            }
            else
            {
                transExistLbl.Visible = false;
                transAltLbl.Text = "";
            }
        }

        private string TransitionAlts(Airport airport)
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

        private void UpdateDataGrid(Airport airport)
        {
            var runways = airport.Rwys;
            airportDataGrid.Columns.Clear();
            airportDataGrid.Rows.Clear();
            airportDataGrid.ColumnCount = 10;
            airportDataGrid.RowCount = runways.Count;
            SetColumnsLables();

            for (int i = 0; i < runways.Count; i++)
            {
                var rwy = runways[i];

                airportDataGrid[0, i].Value = rwy.RwyIdent;
                airportDataGrid[1, i].Value = rwy.Length;
                airportDataGrid[2, i].Value = rwy.Heading;
                airportDataGrid[3, i].Value = rwy.Lat;
                airportDataGrid[4, i].Value = rwy.Lon;

                if (rwy.HasIlsInfo && rwy.IlsAvail)
                {
                    airportDataGrid[5, i].Value = rwy.IlsFreq;
                    airportDataGrid[6, i].Value = rwy.IlsHeading;
                    airportDataGrid[7, i].Value =
                        rwy.GlideslopeAngle.ToString("0.0");
                }
                else
                {
                    airportDataGrid[5, i].Value = "";
                    airportDataGrid[6, i].Value = "";
                    airportDataGrid[7, i].Value = "";
                }

                airportDataGrid[8, i].Value = rwy.Elevation;
                airportDataGrid[9, i].Value = rwy.SurfaceType;
            }

            if (runways.Where(r => r.HasIlsInfo == false).Count() > 0)
            {
                airportDataGrid.Columns.RemoveAt(7);
                airportDataGrid.Columns.RemoveAt(6);
                airportDataGrid.Columns.RemoveAt(5);
            }
        }

        private void SetColumnsLables()
        {
            airportDataGrid.Columns[0].Name = "RWY";
            airportDataGrid.Columns[1].Name = "Length(FT)";
            airportDataGrid.Columns[2].Name = "Heading";
            airportDataGrid.Columns[3].Name = "LAT";
            airportDataGrid.Columns[4].Name = "LON";
            airportDataGrid.Columns[5].Name = "ILS freq";
            airportDataGrid.Columns[6].Name = "ILS course";
            airportDataGrid.Columns[7].Name = "Glideslope angle";
            airportDataGrid.Columns[8].Name = "Threshold altitude(FT)";
            airportDataGrid.Columns[9].Name = "Surface Type";
        }

        public void FindAirport()
        {
            ResetAirport();
            airportDataGrid.Rows.Clear();

            if (CurrentIcao.Length != 4 || Airports == null)
            {
                return;
            }

            var airport = Airports.Find(CurrentIcao);

            if (airport != null && airport.Rwys.Count > 0)
            {
                SetAirport(airport);
                UpdateDataGrid(airport);
                ShowMap(airport.Lat, airport.Lon);
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            SetMetar(CurrentIcao);
        }

        private void icaoComboBox_TextChanged(object sender, EventArgs e)
        {
            FindAirport();
        }

        private void metarLbl_Click(object sender, EventArgs e)
        {
            var frm = new MetarForm();
            frm.icaoTxtBox.Text = CurrentIcao;
            frm.icaoTxtBox.Enabled = false;
            frm.resultRichTxtBox.Text = metarLbl.Text ?? "";
            frm.ShowDialog();
        }

        private void EnableBrowser()
        {
            var wb = new WebBrowser();

            wb.Location = new Point(-3, 270);
            wb.Size = new Size(1021, 384);

            Controls.Add(wb);
            browser = wb;
        }

        private void DisableBrowser()
        {
            Controls.Remove(browser);
            browser = null;
        }

        public void ShowMap(double lat, double lon)
        {
            if (BrowserEnabled)
            {
                ShowMapBrowser(lat, lon);
            }
            else if (StaticMapEnabled)
            {
                ShowStaticMap(lat, lon);
            }
        }

        private void ShowMapBrowser(double lat, double lon)
        {
            // This requires a registry fix. (IE emulation)

            browser.DocumentText = InteractiveMap.GetHtml(
                lat, lon, browser.Width, browser.Height);
        }

        private void EnableStaticMap()
        {
            var pb = new PictureBox();

            pb.Location = new Point(5, 270);
            pb.Size = new Size(1011, 384);
            pb.BackgroundImageLayout = ImageLayout.None;

            // Added so that it doesn't display the default image, 
            // when the first time LoadAsync is called. 
            pb.Image = new Bitmap(1, 1);

            Controls.Add(pb);
            picBox = pb;
        }

        private void DisableStaticMap()
        {
            Controls.Remove(picBox);
            picBox = null;
        }

        private void ShowStaticMap(double lat, double lon)
        {
            picBox.LoadAsync(
                StaticMap.GetMapUrl(
                    lat, lon, picBox.Width, picBox.Height));
        }
    }
}
