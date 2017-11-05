using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.GoogleMap;
using QSP.MathTools;
using QSP.Metar;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Data.Interfaces;
using QSP.UI.Presenters.MiscInfo;
using QSP.UI.Views.Factories;
using QSP.UI.Util;
using QSP.UI.Util.ScrollBar;

namespace QSP.UI.Views.MiscInfo
{
    public partial class AirportMapControl : UserControl, IAirportMapView
    {
        private AirportMapPresenter presenter;
        
        private string CurrentIcao => icaoComboBox.Text.Trim().ToUpper();

        public bool BrowserEnabled
        {
            get { return browser != null; }

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
            get { return picBox != null; }

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
        
        private PictureBox picBox;
        private WebBrowser browser;

        public AirportMapControl()
        {
            InitializeComponent();
        }

        public void Init(AirportMapPresenter presenter)
        {
            this.presenter = presenter;

            ResetAirport();
            SetEmptyDataGrid();

            icaoComboBox.Text = "";

            AddToolTip();
            SetUpdateBtnImage();
            icaoComboBox.UpperCaseOnly();
        }

        private void SetUpdateBtnImage()
        {
            var oldSize = updateBtn.Size;
            var size = new Size(oldSize.Width - 4, oldSize.Height - 4);
            updateBtn.BackgroundImage = ImageUtil.Resize(Properties.Resources.processing, size);
        }
        
        private void AddToolTip()
        {
            var tp = ToolTipFactory.GetToolTip();
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

        private async Task SetMetar()
        {
            await presenter.SetMetar(); 
            metarLbl.Visible = true;
            updateBtn.Visible = true;
        }
        
        private string TransitionAlts(IAirport airport)
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
                    Numbers.RoundToInt(airport.TransLvl / 100.0).ToString();
            }
        }

        private void UpdateDataGrid(IAirport airport)
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
                airportDataGrid[1, i].Value = rwy.LengthFt;
                airportDataGrid[2, i].Value = rwy.Heading;
                airportDataGrid[3, i].Value = rwy.Lat;
                airportDataGrid[4, i].Value = rwy.Lon;

                if (rwy.HasIlsInfo && rwy.IlsAvail)
                {
                    airportDataGrid[5, i].Value = rwy.IlsFreq;
                    airportDataGrid[6, i].Value = rwy.IlsHeading;
                    airportDataGrid[7, i].Value = rwy.GlideslopeAngle.ToString("0.0");
                }
                else
                {
                    airportDataGrid[5, i].Value = "";
                    airportDataGrid[6, i].Value = "";
                    airportDataGrid[7, i].Value = "";
                }

                airportDataGrid[8, i].Value = rwy.ElevationFt;
                airportDataGrid[9, i].Value = rwy.SurfaceType;
            }
            
            SetGridViewHeight();
        }

        private void SetGridViewHeight()
        {
            var view = airportDataGrid;
            var height = 10 + view.ColumnHeadersHeight;
            foreach (DataGridViewRow dr in view.Rows) height += dr.Height;
            view.Height = height;

            // Limit the number of loops to prevent infinite loop.
            for (int i = 0; i < 10; i++)
            {
                if (airportDataGrid.VScrollBarVisible()) airportDataGrid.Height += 10;
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
        
        private void updateBtn_Click(object sender, EventArgs e)
        {
            SetMetar();
        }

        private void icaoComboBox_TextChanged(object sender, EventArgs e)
        {
            ResetAirport();
            airportDataGrid.Rows.Clear();

            if (CurrentIcao.Length != 4 || Airports == null)
            {
                return;
            }

            var airport = Airports[CurrentIcao];

            if (airport != null && airport.Rwys.Count > 0)
            {
                SetMetar(airport.Icao);

                airportNameLbl.Text = airport.Name;
                latLonLbl.Text = airport.Lat.ToString("F6") + " / " + airport.Lon.ToString("F6");
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

                UpdateDataGrid(airport);
                ShowMap(airport.Lat, airport.Lon);
            }
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

            wb.Location = Point.Empty;
            wb.Size = MapSize;
            
            tableLayoutPanel2.Controls.Add(wb, 0, 1);
            browser = wb;
        }

        private Size MapSize => new Size(tableLayoutPanel2.Width, 800);

        public string IcaoText => icaoComboBox.Text.Trim().ToUpper();

        public IEnumerable<string> IcaoList
        {
            set
            {
                icaoComboBox.Items.Clear();
                icaoComboBox.Items.AddRange(value.ToArray());
            }
        }

        public string MetarText { set => throw new NotImplementedException(); }
        public bool TransitionAltExist { set => throw new NotImplementedException(); }
        public string AirportName { set => throw new NotImplementedException(); }
        public string LatLon { set => throw new NotImplementedException(); }
        public string Elevation { set => throw new NotImplementedException(); }
        public string TransitionAltLevel { set => throw new NotImplementedException(); }

        private void DisableBrowser()
        {
            tableLayoutPanel2.Controls.Remove(browser);
            browser = null;
        }

        public void ShowMap(ICoordinate c)
        {
            if (BrowserEnabled)
            {
                ShowMapBrowser(c.Lat, c.Lon);
            }
            else if (StaticMapEnabled)
            {
                ShowStaticMap(c.Lat, c.Lon);
            }
        }

        private void ShowMapBrowser(double lat, double lon)
        {
            // This requires a registry fix. (IE emulation)

            browser.DocumentText = InteractiveMap.GetHtml(lat, lon, browser.Width, browser.Height);
        }

        private void EnableStaticMap()
        {
            var pb = new PictureBox();

            pb.Location = Point.Empty;
            pb.Size = MapSize;
            pb.BackgroundImageLayout = ImageLayout.None;

            // Added so that it doesn't display the default image, 
            // when the first time LoadAsync is called. 
            pb.Image = new Bitmap(1, 1);

            tableLayoutPanel2.Controls.Add(pb);
            picBox = pb;
        }

        private void DisableStaticMap()
        {
            tableLayoutPanel2.Controls.Remove(picBox);
            picBox = null;
        }

        private void ShowStaticMap(double lat, double lon)
        {
            picBox.LoadAsync(StaticMap.GetMapUrl(lat, lon, picBox.Width, picBox.Height));
        }
    }
}
